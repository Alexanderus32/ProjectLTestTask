using Core;
using Core.Interfaces;
using System.Linq;


namespace ProjectLTestTask.Commands
{
    class ClientAudioCommand : ICommand
    {
        private readonly IVolumeService volumeService;

        public ClientAudioCommand(IVolumeService volumeService)
        {
            this.volumeService = volumeService;
        }

        public CommandConstants[] Commands { get => new CommandConstants[] {  }; }

        public ServiceType Type => ServiceType.AudioService;

        public string Execute(IMessage command)
        {
            var isVolume = int.TryParse(command.Payload.FirstOrDefault(x => x.Key == "Volume").Value, out int volume);
            if (isVolume)
            {
                this.volumeService.SetVolume(volume);
            }
            return null;
        }
    }
}
