using AudioSwitcher.AudioApi.CoreAudio;
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
    //Test server console app
    class ServerObserver : IPipeStreamObserver<string>
    {

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
                CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
                defaultPlaybackDevice.Volume = double.Parse(value);
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

        public void GetCurrentVolume()
        {
            PipeStream.Write(CommandConstants.GetVolume.ToString());
        }

        public void ReturnVolume()
        {
            CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
            int currentVolume = (int)defaultPlaybackDevice.Volume;
            PipeStream.Write(currentVolume.ToString());
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
