using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService1.Interfaces
{
    public interface ISend
    {
        PipeStream PipeStream { get; set; }

        void Send(string message);
    }
}
