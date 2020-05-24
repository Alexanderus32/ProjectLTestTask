using Core.NamedPipes;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService1
{
    class ServerObserver : IPipeStreamObserver<string>
    {

        public void OnNext(string value)
        {
            value = "Client: " + value;
            //Console.WriteLine(value);
            //PipeStream.Write(value);
        }

        public void OnError(Exception error)
        {

        }

        public void OnCompleted()
        {

        }

        public PipeStream PipeStream { get; set; }

        public void OnConnected()
        {
            PipeStream.Write("Connected");
        }

        public void Say(string value)
        {
            PipeStream.Write(value);
        }
    }
}
