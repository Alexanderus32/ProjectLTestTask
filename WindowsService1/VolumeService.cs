using AudioSwitcher.AudioApi.CoreAudio;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading;

namespace WindowsService1
{
    public class VolumeService : IVolumeService
    {

        public event EventHandler<ValueEventArgs<string>> ChangeVolume;

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
                    OnChangeVolume(new ValueEventArgs<string>(systemVolume.ToString()));
                    
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

        protected virtual void OnChangeVolume(ValueEventArgs<string> e)
        {
            ChangeVolume?.Invoke(this, e);
        }


    }
}
