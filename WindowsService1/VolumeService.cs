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
        public event EventHandler ChangeVolume;

        private CoreAudioDevice defaultPlaybackDevice;

        private int systemVolume;

        public VolumeService()
        {
            CreateDevice();
            this.systemVolume = (int)defaultPlaybackDevice.Volume;
        }

        private void CreateDevice()
        {
            this.defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
            this.defaultPlaybackDevice.VolumeChanged.Subscribe(device => {
                if(systemVolume != device.Volume)
                {
                    this.systemVolume = (int)device.Volume;
                    defaultPlaybackDevice.Volume = systemVolume;
                    OnChangeVolume(new EventArgs());
                }
            
            });
        }

        public int GetCurrentVolume()
        {
            return this.systemVolume;
        }

        public void SetVolume(int value)
        {
            this.defaultPlaybackDevice.Volume = value;
            systemVolume = value;
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
