using Core;
using Core.NamedPipes;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    //Test server and client console app
    class ServerObserver : IPipeStreamObserver<string>
    {

        public void OnNext(string value)
        {
            value = "Client: "+value;
            Console.WriteLine(value);
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

    class ClientObserver : IPipeStreamObserver<string>
    {

        public void OnNext(string value)
        {
            value = "Server: " + value;
            Console.WriteLine(value);
        }

        public void OnError(Exception error)
        {
            Console.WriteLine(error);
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
    class Program
    {
        static void Main(string[] args)
        {
            var server = new IpcServer<string>("test");
            server.Start<ServerObserver>();
            //var client1 = new IpcClient<string>(".", "test", new ClientObserver());
            //var client1Worked = client1.Create();
            //Console.ReadLine();
            //client1.Observer.Say("hello");
            //Console.ReadLine();
            //client1.Observer.Say("qqq");
            //Console.ReadLine();
            //Console.ReadLine();
            //client1Worked.Dispose();
            //Console.WriteLine("Client1 disposed!");
            //Console.ReadLine();
            //var client2 = new IpcClient<string>(".", "test", new ClientObserver()).Create();

            //Console.ReadLine();
            //client2.Dispose();
            //Console.WriteLine("Client2 disposed!");
            //Console.ReadLine();
            //var client3 = new IpcClient<string>(".", "test", new ClientObserver()).Create();

            //Console.ReadLine();
            //client3.Dispose();
            //Console.WriteLine("Client3 disposed!");

            //Console.ReadLine();
            //server.Stop();
            //Console.WriteLine("Server stopped!");
            Console.ReadLine();
        }
    }
}
