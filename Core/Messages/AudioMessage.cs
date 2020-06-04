using Core.Interfaces;
using System;
using System.Collections.Generic;

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
