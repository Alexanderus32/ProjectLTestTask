using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IMessage
    {
        ServiceType Type { get; set; }

        Dictionary<string, string> Payload { get; set; }
    }
}
