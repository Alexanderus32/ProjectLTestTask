using Core.Interfaces;
using System.Collections.Generic;

namespace Core
{
    public class LogMessage : IMessage
    {
        public ServiceType Type { get; set; }

        public Dictionary<string, string> Payload { get; set; }

        public LogMessage()
        {
            Type = ServiceType.LogSevice; 
        }
    }
}
