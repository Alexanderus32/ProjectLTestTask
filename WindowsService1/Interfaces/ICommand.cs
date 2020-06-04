using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService1.Interfaces
{
    public interface ICommand
    {
        CommandConstants[] Commands { get; }

        ServiceType Type { get; }

        string Execute(IMessage command);
    }
}
