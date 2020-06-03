using Core;
using Core.NamedPipes;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsService1.Interfaces;

namespace WindowsService1
{
    class Sender : ISend
    {

        public PipeStream PipeStream { get; set; }

        private readonly IVolumeService volumeService;

        public Sender(IVolumeService volServ)
        {
            this.volumeService = volServ;
            this.volumeService.ChangeVolume += new EventHandler<ValueEventArgs<string>>(VolumeChanged);
        }

        public void Send(string message)
        {
            PipeStream?.Write(message);
        }

        private void VolumeChanged(object source, ValueEventArgs<string> e)
        {
            Send(e.Value);
        }
    }
}
