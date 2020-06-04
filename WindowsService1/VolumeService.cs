using AudioSwitcher.AudioApi.CoreAudio;
using Core;
using Core.Interfaces;
using System;

namespace WindowsService1
{
    class VolumeService : IVolumeService
    {

        public event EventHandler<ValueEventArgs<int>> ChangeVolume;

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
                    OnChangeVolume(new ValueEventArgs<int>(systemVolume));
                    
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

        protected virtual void OnChangeVolume(ValueEventArgs<int> e)
        {
            ChangeVolume?.Invoke(this, e);
        }


    }
}
