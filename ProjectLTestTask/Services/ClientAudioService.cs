using Core;
using Core.Interfaces;
using System;

namespace ProjectLTestTask.Services
{
    class ClientAudioService : IVolumeService
    {
        public event EventHandler<ValueEventArgs<int>> ChangeVolume;

        public int GetCurrentVolume()
        {
            throw new NotImplementedException();
        }

        public void SetVolume(int value)
        {
            OnChangeVolume(new ValueEventArgs<int>(value));
        }

        protected virtual void OnChangeVolume(ValueEventArgs<int> e)
        {
            ChangeVolume?.Invoke(this, e);
        }
    }
}
