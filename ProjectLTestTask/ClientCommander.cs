using CommonServiceLocator;
using Core;
using Newtonsoft.Json;
using ProjectLTestTask.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsService1;
using WindowsService1.Interfaces;

namespace ProjectLTestTask
{
    class ClientCommander : ICommander
    {
        private IReadOnlyList<ICommand> commands;

        private readonly ISend sender;

        public ClientCommander(ISend sender)
        {
            this.sender = sender;
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            commands = new List<ICommand> {
                new ClientAudioCommand(ServiceLocator.Current.GetInstance<IVolumeService>())
            };
        }

        public void Execute(string args)
        {
            JsonConverter[] converters = { new MessageConverter() };
            var message = JsonConvert.DeserializeObject<IMessage>(args, new JsonSerializerSettings() { Converters = converters });
            if (message != null)
            {
                var result = commands.FirstOrDefault(x => x.Type == message.Type)?.Execute(message);
                if (result != null)
                {
                    this.sender.Send(result);
                }
            }

        }
    }
}
