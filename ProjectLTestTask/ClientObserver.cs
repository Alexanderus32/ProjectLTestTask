using CommonServiceLocator;
using Core;
using Core.Interfaces;
using Core.NamedPipes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Pipes;

namespace ProjectLTestTask
{
    public class ClientObserver : IPipeStreamObserver<string>
    {
        private readonly ICommander commander;

        private readonly ISend sender;

        public PipeStream PipeStream { get; set; }

        public ClientObserver()
        {
            this.commander = ServiceLocator.Current.GetInstance<ICommander>();
            this.sender = ServiceLocator.Current.GetInstance<ISend>();
        }


        public void OnNext(string value)
        {       
            commander.Execute(value);
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
        }

    }
}
