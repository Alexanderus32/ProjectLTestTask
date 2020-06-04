using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLTestTask
{
    public interface INotificator
    {
        event EventHandler<ValueEventArgs<string>> Notify;

        void Log(string message);
    }
}
