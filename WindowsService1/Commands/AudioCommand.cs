using Core;

namespace WindowsService1
{
    class AudioCommand : Interfaces.Command
    {
        private readonly IVolumeService volumeService;

        public AudioCommand(IVolumeService volumeService)
        {
            this.volumeService = volumeService;
        }

        public override CommandConstants[] Commands { get => new CommandConstants[] { CommandConstants.GetVolume };  }

        public override string Execute(string command)
        {
            if(command == CommandConstants.GetVolume.ToString())
            {
                var result = this.volumeService.GetCurrentVolume();
                return result.ToString();
            }
            else
            {
                int.TryParse(command, out int volume);
                this.volumeService.SetVolume(volume);
                return null;
            }
        }

        public override bool IsThisCommand(string args)
        {
            var result =  base.IsThisCommand(args);
            if (result)
            {
                return result;
            }
            else
            {
                result = int.TryParse(args, out int volume);
            }
            return result;
                
       }
    }
}
