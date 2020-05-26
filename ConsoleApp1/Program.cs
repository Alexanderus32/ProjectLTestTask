using AudioSwitcher.AudioApi.CoreAudio;
using Core;
using Core.NamedPipes;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsService1;

namespace ConsoleApp1
{
    //Test server console app
    class ServerObserver : IPipeStreamObserver<string>
    {
        IVolumeService volumeService;

        public ServerObserver()
        {
            volumeService = new VolumeService();
            this.volumeService.ChangeVolume += new EventHandler(VolumeChanged);
        }

        private void VolumeChanged(object source, EventArgs e)
        {
            Console.WriteLine(this.volumeService.GetCurrentVolume().ToString());
           // PipeStream.Write(this.volumeService.SystemVolume.ToString());
        }

        public void OnNext(string value)
        {
            Enum.TryParse(value, out CommandConstants commands);
            switch (commands)
            {
                case CommandConstants.GetVolume:
                    ReturnVolume();
                    break;
                default:
                    break;
            }
            bool isVolume = int.TryParse(value, out int volume);
            if (isVolume)
            {
                volumeService.SetVolume(volume);
            }
            value = "Client: " + value;
            Console.WriteLine(value);
            PipeStream.Write(value);
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

        public void ReturnVolume()
        {   
            PipeStream.Write(this.volumeService.GetCurrentVolume().ToString());
        }

        public void GetCurrentVolume()
        {
            throw new NotImplementedException();
        }

        public void SetCurrentVolume(int value)
        {
            throw new NotImplementedException();
        }
    }

    
    class Program
    {
        static void Main(string[] args)
        {
            var server = new IpcServer<string>("test");
            var serverObserver = new ServerObserver();
            server.Start<ServerObserver>();
            Console.ReadLine();
        }
    }
}
