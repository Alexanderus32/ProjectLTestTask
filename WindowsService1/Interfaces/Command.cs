using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService1.Interfaces
{
    abstract class Command
    {
        public abstract CommandConstants[] Commands { get; }

        public virtual bool IsThisCommand(string args)
        {
            foreach (var command in Commands)
            {
                if (command.ToString() == args)
                {
                    return true;
                }
            }
            return false;
        }

        public abstract string Execute(string command);
    }
}
