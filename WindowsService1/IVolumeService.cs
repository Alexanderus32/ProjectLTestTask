using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService1
{
    public interface IVolumeService
    {
        event EventHandler ChangeVolume;

        int GetCurrentVolume();

        void SetVolume(int value);
    }
}
