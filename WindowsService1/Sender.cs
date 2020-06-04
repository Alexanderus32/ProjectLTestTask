using Core;
using Core.Interfaces;
using Core.NamedPipes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Pipes;

namespace WindowsService1
{
    public class Sender : ISend
    {

        public PipeStream PipeStream { get; set; }

        private readonly IVolumeService volumeService;

        public Sender(IVolumeService volServ)
        {
            this.volumeService = volServ;
            this.volumeService.ChangeVolume += new EventHandler<ValueEventArgs<int>>(VolumeChanged);
        }

        public void Send(string message)
        {
            PipeStream?.Write(message);
        }

        public void Connected()
        {
            var dictionary = new Dictionary<string, string> { { "Message", "Connected" } };
            string json = MessageConverter.CreateJson(new LogMessage(), dictionary);
            Send(json);
        }

        private void VolumeChanged(object source, ValueEventArgs<int> e)
        {
            var dictionary = new Dictionary<string, string> { { "Volume", e.Value.ToString() } };
            string json = MessageConverter.CreateJson(new AudioMessage(), dictionary);
            Send(json);
        }
    }
}
