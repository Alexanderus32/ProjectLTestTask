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

        private readonly ISend sender;

        public Commander(ISend sender)
        {
            this.sender = sender;
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            commands = new List<Command> { 
                new AudioCommand(IoCRegister.Container.GetInstance<IVolumeService>())
            };
        }

        public void Execute(string args)
        {
            foreach (var command in commands)
            {
                if (command.IsThisCommand(args))
                {
                    string result = command.Execute(args);

                    if (result != null)
                    {
                        this.sender.Send(result);
                    }

                    return;
                }
            }
        }
    }
}
