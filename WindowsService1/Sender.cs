using Core;
using Core.NamedPipes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsService1.Interfaces;

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

        private void VolumeChanged(object source, ValueEventArgs<int> e)
        {
            var dictionary = new Dictionary<string, string> { { "Volume", e.Value.ToString() } };
            string json = MessageConverter.CreateJson(new AudioMessage(), dictionary);
            Send(json);
        }
    }
}
