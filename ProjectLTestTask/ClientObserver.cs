using Core.NamedPipes;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLTestTask
{
    class ClientObserver : IPipeStreamObserver<string>
    {
        public delegate void VolumeHandler(int volume);
        public event VolumeHandler ChangeVolume;

        public delegate void NotifyHandler(string value);
        public event NotifyHandler Notify;

        private bool volumeChanged;
        private int volume;

        public void OnNext(string value)
        {
            if (value == CommandConstants.Default.ToString())
            {
                if (volumeChanged)
                {
                    volumeChanged = false;
                    PipeStream.Write(volume.ToString());
                }
                else
                {
                    PipeStream.Write(CommandConstants.Default.ToString());
                }
            }
            else
            {               
                bool isVolume = int.TryParse(value, out int volume);
                if (isVolume)
                {
                    ChangeVolume?.Invoke(volume);
                    PipeStream.Write(CommandConstants.Default.ToString());
                }
                value = "Server: " + value;
                Task.Run(() => Notify?.Invoke(value));
                PipeStream.Write(CommandConstants.Default.ToString());
            }
        }

        public void OnError(Exception error)
        {
        }

        public void OnCompleted()
        {
            PipeStream.Write("Completed");
        }

        public PipeStream PipeStream { get; set; }

        public void OnConnected()
        {
            PipeStream.Write(CommandConstants.Default.ToString());
        }

        public void Say(string value)
        {
            PipeStream.Write(value);
        }

        public void GetCurrentVolume()
        {
            PipeStream.Write(CommandConstants.GetVolume.ToString());
        }

        public void SetCurrentVolume(int value)
        {
            volumeChanged = true;
            volume = value;
        }

    }
}
