using AudioSwitcher.AudioApi.CoreAudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading;

namespace WindowsService1
{
    public class VolumeService : IVolumeService
    {
        private static Timer time;

        private static TimerCallback timerCallback;

        public event EventHandler ChangeVolume;

        private int systemVolume;

        private CoreAudioDevice defaultPlaybackDevice;

        public VolumeService()
        {
            defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
            defaultPlaybackDevice.VolumeChanged.Subscribe(observer => {
                try
                {
                    defaultPlaybackDevice.Volume = observer.Volume;
                }
                catch { }
            });
            this.systemVolume = GetCurrentVolume();
            timerCallback = new TimerCallback(TimerEvent);
            time = new Timer(timerCallback, null, 0, 3000);
        }

        public int GetCurrentVolume()
        {
            return (int)defaultPlaybackDevice.Volume;
        }

        public void SetVolume(int value)
        {
            defaultPlaybackDevice.Volume = value;
        }

        private void TimerEvent(object state)
        {
            int currentVolume = GetCurrentVolume();
            if (this.systemVolume != currentVolume)
            {
                this.systemVolume = currentVolume;
                OnChangeVolume(new EventArgs());
            }
        }

        private void OnChangeVolume(EventArgs e)
        {        
            if (ChangeVolume != null)
            {
                ChangeVolume.Invoke(this, e);
            }
        }


    }
}
