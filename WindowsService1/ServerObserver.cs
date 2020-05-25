using AudioSwitcher.AudioApi.CoreAudio;
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
        private readonly IVolumeService volumeService;

        public ServerObserver()
        {
            this.volumeService = new VolumeService();
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

        public void GetCurrentVolume()
        {
            PipeStream.Write(CommandConstants.GetVolume.ToString());
        }

        public void ReturnVolume()
        {   
            PipeStream.Write(volumeService.GetCurrentVolume().ToString());
        }

        public void SetCurrentVolume(int value)
        {
            throw new NotImplementedException();
        }
    }
}
