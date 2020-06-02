using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsService1.Interfaces;

namespace WindowsService1
{
    class Commander : ICommander
    {
        private IReadOnlyList<Command> commands;

        public Commander()
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            commands = new List<Command> { 
                new AudioCommand(IoCRegister.Container.GetInstance<IVolumeService>())
            };
        }

        public string Execute(string args)
        {
            foreach (var command in commands)
            {
                if (command.IsThisCommand(args))
                {
                    return command.Execute(args);
                }
            }
            return null;
        }
    }
}
