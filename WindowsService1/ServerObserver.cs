using AudioSwitcher.AudioApi.CoreAudio;
using Core;
using Core.NamedPipes;
using System;
using System.Collections.Generic;
using System.IO.Pipes;

namespace WindowsService1
{
    class ServerObserver : IPipeStreamObserver<string>
    {
        private readonly ICommander commander;

        public PipeStream PipeStream { get; set; }

        public ServerObserver()
        {
            this.commander = IoCRegister.Container.GetInstance<ICommander>();
        }

        public void OnNext(string value)
        {
            PipeStream.Write("Client: " + value);
            var result = commander.Execute(value);
            if (result != null)
            {
                PipeStream.Write(result);
            }
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
            //ReturnVolume();
        }

        public void Say(string value)
        {
            PipeStream.Write(value);
        }

        //public void ReturnVolume()
        //{
        //  //  PipeStream.Write(this.volumeService.GetCurrentVolume().ToString());
        //}

        //public void VolumeChangeHandler(int value)
        //{
        //  //  PipeStream.Write(this.volumeService.GetCurrentVolume().ToString());
        //}
    }
}
