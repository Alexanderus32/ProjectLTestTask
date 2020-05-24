using Core.NamedPipes;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLTestTask
{
    class ClientObserver : IPipeStreamObserver<string>
    {

        public void OnNext(string value)
        {
            value = "Server: " + value;
            //Console.WriteLine(value);
        }

        public void OnError(Exception error)
        {
           // Console.WriteLine(error);
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
