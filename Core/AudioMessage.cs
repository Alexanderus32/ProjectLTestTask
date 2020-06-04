using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class AudioMessage : IMessage
    {
        public ServiceType Type { get; set; }

        public AudioMessage()
        {
            Type = ServiceType.AudioService;
        }

        public Dictionary<string, string> Payload { get; set; }
    }
}
