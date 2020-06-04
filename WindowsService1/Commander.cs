using Core;
using Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WindowsService1
{
    class Commander : ICommander
    {
        private IReadOnlyList<ICommand> commands;

        private readonly ISend sender;

        public Commander(ISend sender)
        {
            this.sender = sender;
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            commands = new List<ICommand> { 
                new AudioCommand(IoCRegister.Container.GetInstance<IVolumeService>())
            };
        }

        public void Execute(string args)
        {
            JsonConverter[] converters = { new MessageConverter() };
            var message = JsonConvert.DeserializeObject<IMessage>(args, new JsonSerializerSettings() { Converters = converters });
            //IMessage message = null;
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
