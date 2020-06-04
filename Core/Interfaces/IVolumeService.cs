using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IVolumeService
    {
        event EventHandler<ValueEventArgs<int>> ChangeVolume;

        int GetCurrentVolume();

        void SetVolume(int value);
    }
}
