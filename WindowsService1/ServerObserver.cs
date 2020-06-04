using AudioSwitcher.AudioApi.CoreAudio;
using Core;
using Core.NamedPipes;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using WindowsService1.Interfaces;

namespace WindowsService1
{
    class ServerObserver : IPipeStreamObserver<string>
    {
        private readonly ICommander commander;

        private readonly ISend sender;

        public PipeStream PipeStream { get; set; }

        public ServerObserver()
        {
            this.commander = IoCRegister.Container.GetInstance<ICommander>();
            this.sender = IoCRegister.Container.GetInstance<ISend>();
        }

        public void OnNext(string value)
        {
            //this.sender.Send("Client: " + value);
            this.commander.Execute(value);
        }


        public void OnError(Exception error)
        {

        }

        public void OnCompleted()
        {

        }

        public void OnConnected()
        {
            this.sender.PipeStream = PipeStream;
            this.sender.Send("Connected");
        }

    }
}
