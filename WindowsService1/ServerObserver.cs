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
        IVolumeService volumeService;

        public PipeStream PipeStream { get; set; }

        public ServerObserver()
        {
            volumeService = new VolumeService();
            this.volumeService.ChangeVolume += new EventHandler(VolumeChanged);
        }

        private void VolumeChanged(object source, EventArgs e)
        {
            Console.WriteLine(this.volumeService.GetCurrentVolume().ToString());
            PipeStream.Write(this.volumeService.GetCurrentVolume().ToString());
        }

        public void OnNext(string value)
        {
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

        public void OnConnected()
        {
            PipeStream.Write("Connected");
            ReturnVolume();
        }

        public void Say(string value)
        {
            PipeStream.Write(value);
        }

        public void ReturnVolume()
        {
            PipeStream.Write(this.volumeService.GetCurrentVolume().ToString());
        }

        public void SetCurrentVolume(int value)
        {
            PipeStream.Write(this.volumeService.GetCurrentVolume().ToString());
        }
    }
}
