using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.NamedPipes
{
    public interface IPipeStreamObserver<in T> : IObserver<T>
    {
        PipeStream PipeStream { get; set; }

        void OnConnected();

        void Say(string value);

        void SetCurrentVolume(int value);

    }
}
