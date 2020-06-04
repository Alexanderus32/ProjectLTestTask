using CommonServiceLocator;
using Core;
using Core.Interfaces;
using Newtonsoft.Json;
using ProjectLTestTask.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectLTestTask
{
    class ClientCommander : ICommander
    {
        private IReadOnlyList<ICommand> commands;

        private readonly ISend sender;

        private readonly INotificator notificator;

        public ClientCommander(ISend sender, INotificator notificator)
        {
            this.sender = sender;
            this.notificator = notificator;
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            commands = new List<ICommand> {
                new ClientAudioCommand(ServiceLocator.Current.GetInstance<IVolumeService>()),
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
                string serverMessage = $"Server:\n Type:{message.Type}\n";
                foreach (var item in message.Payload)
                {
                    serverMessage += $"{item.Key}: {item.Value}\n";
                }
                this.notificator.Log(serverMessage);
            }
        }
    }
}
