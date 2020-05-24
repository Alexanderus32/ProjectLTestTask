using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.NamedPipes
{
    public class IpcServer<T>
    {
        public IPipeStreamObserver<T> Observer { get; set; }
        private bool _running;
        private readonly AutoResetEvent _auto = new AutoResetEvent(false);
        private readonly List<PipeStream> _pipes = new List<PipeStream>();
        private readonly string _name;
        private Task _task;

        public IpcServer(string name)
        {
            _name = name;
            _task = Task.Factory.StartNew(() => { });
        }

        public void IpcServerPipeCreate<TPipeStreamObserver>(Task task)
            where TPipeStreamObserver : IPipeStreamObserver<T>, new()
        {

            NamedPipeServerStream pipe;
            var observable = PipeStreamObservable.Create<T>(out pipe, _name, (sender, args) => _auto.Set());

            Observer = new TPipeStreamObserver() { PipeStream = pipe };
            observable.Subscribe(
                                Observer.OnNext,
                                ex =>
                                {
                                    Observer.OnError(ex);
                                    pipe.Close();
                                    lock (_pipes)
                                    {
                                        _pipes.Remove(pipe);
                                    }
                                },
                                () =>
                                {
                                    Observer.OnCompleted();
                                    pipe.Close();
                                    lock (_pipes)
                                    {
                                        _pipes.Remove(pipe);
                                    }
                                });

            _auto.WaitOne();

            lock (_pipes)
            {
                if (_running)
                    _pipes.Add(pipe);
            }

            if (_running)
            {
                task.ContinueWith(IpcServerPipeCreate<TPipeStreamObserver>);
                Observer.OnConnected();
            }
            else
            {
                pipe.Close();
            }

        }

        public Task Start<TPipeStreamObserver>() where TPipeStreamObserver : IPipeStreamObserver<T>, new()
        {
            _running = true;
            _task = _task.ContinueWith(IpcServerPipeCreate<TPipeStreamObserver>);
            //Observer = new TPipeStreamObserver();
            return _task;
        }

        public void Stop()
        {
            _running = false;
            _task.ContinueWith(Stop);
        }

        private void Stop(Task task)
        {
            lock (_pipes)
            {
                _running = false;
                _pipes.ForEach(pipe => pipe.Close());
            }
            _auto.Set();
            for (; ; )
            {
                int count;
                lock (_pipes)
                {
                    count = _pipes.Count;
                }
                if (count == 0)
                    break;
            }
        }
    }
}
