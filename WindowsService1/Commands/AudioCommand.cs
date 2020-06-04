using Core;
using System.Text.Json;
using System.Collections.Generic;
using WindowsService1.Interfaces;
using Newtonsoft.Json;
using System.Linq;

namespace WindowsService1
{
    class AudioCommand : ICommand
    {
        private readonly IVolumeService volumeService;

        public AudioCommand(IVolumeService volumeService)
        {
            this.volumeService = volumeService;
        }

        public CommandConstants[] Commands { get => new CommandConstants[] { CommandConstants.GetVolume };  }

        public ServiceType Type => ServiceType.AudioService;

        public string Execute(IMessage command)
        {
            if(command.Payload.Any(x=>x.Value == CommandConstants.GetVolume.ToString()))
            {
                var currentVolume = this.volumeService.GetCurrentVolume();

                var dictionary = new Dictionary<string, string> { { "Volume", currentVolume.ToString() } };
                string json = MessageConverter.CreateJson(new AudioMessage(),  dictionary);

                return json;
            }
            else
            {
                int.TryParse(command.Payload.FirstOrDefault(x=>x.Key == "Volume").Value, out int volume);
                this.volumeService.SetVolume(volume);
                return null;
            }
        }
    }
}
