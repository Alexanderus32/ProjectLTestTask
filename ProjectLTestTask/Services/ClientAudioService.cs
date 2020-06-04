using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsService1;

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
