using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.NamedPipes
{
    public static class PipeStreamObservable
    {
        private static readonly PipeSecurity PipeSecurity;

        private static readonly ThreadLocal<BinaryFormatter> BinaryFormatter
            = new ThreadLocal<BinaryFormatter>(() => new BinaryFormatter());

        static PipeStreamObservable()
        {
            PipeSecurity = new PipeSecurity();
            PipeSecurity.AddAccessRule(
                new PipeAccessRule(WindowsIdentity.GetCurrent().User, PipeAccessRights.FullControl, AccessControlType.Allow)
            );
            PipeSecurity.AddAccessRule(
                new PipeAccessRule(
                    new SecurityIdentifier(WellKnownSidType.WorldSid, null), PipeAccessRights.ReadWrite,
                    AccessControlType.Allow
                    )
                );
        }

        public static void Write<T>(this PipeStream stream, T type)
        {
            BinaryFormatter.Value.Serialize(stream, type);
        }

        public static IObservable<T> Create<T>(out NamedPipeServerStream stream, string pipeName,
            EventHandler onConnected = null)
        {
            stream = new NamedPipeServerStream(pipeName, PipeDirection.InOut,
                -1, PipeTransmissionMode.Byte,
                PipeOptions.Asynchronous, 4096, 4096, PipeSecurity);

            var serverStream = stream;
            return Observable.Using(() => serverStream, resource => Observable.Create<T>(o =>
            {

                var currentStateSubscription = new SerialDisposable();

                return NewThreadScheduler.Default.Schedule(
                    new FormatterIterator<T>(resource, new BinaryFormatter(), onConnected), (state, self) =>
                    {
                        var observable = state.ReadNext();
                        if (observable != null)
                            currentStateSubscription.Disposable =
                                observable.Subscribe(
                                    str =>
                                    {
                                        self(state);
                                        o.OnNext(str);
                                    },
                                    ex =>
                                    {
                                        currentStateSubscription
                                            .Dispose();
                                        o.OnError(ex);
                                        resource.Close();
                                    },
                                    () =>
                                    {
                                        currentStateSubscription
                                            .Dispose();
                                        o.OnCompleted();
                                        resource.Close();

                                    });
                        else
                            o.OnError(new Exception("Scheduling aborted."));
                    }
                    );
            }));
        }

        public static IObservable<T> Create<T>(out NamedPipeClientStream stream,
            string server,
            string pipeName,
            EventHandler onConnected = null)
        {
            stream = new NamedPipeClientStream(server, pipeName, PipeDirection.InOut,
                PipeOptions.Asynchronous);

            var clientStream = stream;
            return Observable.Using(() => clientStream, resource => Observable.Create<T>(o =>
            {

                var currentStateSubscription = new SerialDisposable();

                return NewThreadScheduler.Default.Schedule(
                    new FormatterIterator<T>(resource, new BinaryFormatter(), onConnected), (state, self) =>
                    {
                        var observable = state.ReadNext();
                        if (observable != null)
                            currentStateSubscription.Disposable =
                                observable.Subscribe(
                                    str =>
                                    {
                                        self(state);
                                        o.OnNext(str);
                                    },
                                    ex =>
                                    {
                                        currentStateSubscription
                                                .Dispose();
                                        o.OnError(ex);
                                        resource.Close();
                                    },
                                    () =>
                                    {
                                        currentStateSubscription
                                                .Dispose();
                                        o.OnCompleted();
                                        resource.Close();
                                    });
                        else
                            o.OnError(new Exception("Scheduling aborted."));
                    }
                    );
            }));
        }

        private delegate bool StreamHandler<T>(Stream input, out T output);


        private abstract class ReaderState<T> : IIterator<T>
        {
            public abstract IObservable<T> ReadNext();
        }

        private class ReadReadyState<T> : ReaderState<T>
        {
            private readonly PipeStream _stream;
            private readonly StreamHandler<T> _handler;


            internal ReadReadyState(PipeStream stream, StreamHandler<T> handler)
            {
                _stream = stream;
                _handler = handler;
            }

            public override IObservable<T> ReadNext()
            {
                return Observable.Create<T>(o =>
                {
                    try
                    {
                        if (_stream.IsConnected)
                        {
                            T value;
                            if (_handler(_stream, out value))
                                o.OnNext(value);
                           
                        }
                    }
                    catch (Exception e)
                    {
                        o.OnError(e);
                    }

                    return Disposable.Empty;
                });
            }
        }

        private class ServerStreamReader<T> : IIterator<T>
        {
            private ReaderState<T> _currentState;
            private readonly NamedPipeServerStream _stream;
            private readonly StreamHandler<T> _handler;

            internal ServerStreamReader(NamedPipeServerStream stream, StreamHandler<T> handler, EventHandler onConnected)
            {
                _stream = stream;
                _handler = handler;
                _currentState = new ConnectionWaitState<T>(this, onConnected);
            }

            private class ConnectionWaitState<T1> : ReaderState<T1>
            {
                private readonly ServerStreamReader<T1> _parent;
                private readonly EventHandler _onConnected;

                internal ConnectionWaitState(ServerStreamReader<T1> parent, EventHandler onConnected)
                {
                    _parent = parent;
                    _onConnected += onConnected;
                }

                public override IObservable<T1> ReadNext()
                {
                    try
                    {
                        _parent._stream.WaitForConnection();
                        if (_onConnected != null)
                            _onConnected(this, EventArgs.Empty);
                        _parent._currentState = new ReadReadyState<T1>(_parent._stream, _parent._handler);
                        return _parent._currentState.ReadNext();
                    }
                    catch (Exception)
                    {

                    }
                    return null;
                }
            }

            public IObservable<T> ReadNext()
            {
                return _currentState.ReadNext();
            }
        }

        private class ClientStreamReader<T> : IIterator<T>
        {
            private ReaderState<T> _currentState;
            private readonly NamedPipeClientStream _stream;
            private readonly StreamHandler<T> _handler;

            internal ClientStreamReader(NamedPipeClientStream stream, StreamHandler<T> handler, EventHandler onConnected)
            {
                _stream = stream;
                _handler = handler;
                _currentState = new ConnectionWaitState<T>(this, onConnected);
            }

            private class ConnectionWaitState<T1> : ReaderState<T1>
            {
                private readonly ClientStreamReader<T1> _parent;
                private readonly EventHandler _onConnected;

                internal ConnectionWaitState(ClientStreamReader<T1> parent, EventHandler onConnected)
                {
                    _parent = parent;
                    _onConnected += onConnected;
                }

                public override IObservable<T1> ReadNext()
                {
                    try
                    {
                        _parent._stream.Connect(5000);
                        if (_onConnected != null)
                            _onConnected(this, EventArgs.Empty);
                        _parent._currentState = new ReadReadyState<T1>(_parent._stream, _parent._handler);
                        return _parent._currentState.ReadNext();
                    }
                    catch (Exception)
                    {

                    }
                    return null;
                }
            }

            public IObservable<T> ReadNext()
            {
                return _currentState.ReadNext();
            }
        }

        private sealed class FormatterIterator<T> : IIterator<T>
        {
            private readonly IIterator<T> _iterator;
            private readonly IFormatter _formatter;

            public FormatterIterator(NamedPipeServerStream source, IFormatter formatter, EventHandler onConnected)
            {
                _iterator = new ServerStreamReader<T>(source, DeserializeWithFormatter, onConnected);
                _formatter = formatter;
            }

            public FormatterIterator(NamedPipeClientStream source, IFormatter formatter, EventHandler onConnected)
            {
                _iterator = new ClientStreamReader<T>(source, DeserializeWithFormatter, onConnected);
                _formatter = formatter;
            }

            public IObservable<T> ReadNext()
            {
                return _iterator.ReadNext();
            }

            private bool DeserializeWithFormatter(Stream stream, out T value)
            {
                try
                {
                    value = (T)_formatter.Deserialize(stream);
                    return true;
                }
                catch (Exception ex)
                {
                    value = default(T);
                    return false;
                }
            }
        }
    }
}
