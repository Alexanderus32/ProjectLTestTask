using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLTestTask
{
    class Notificator : INotificator
    {
        public event EventHandler<ValueEventArgs<string>> Notify;

        public void Log(string message)
        {
            OnNotify(new ValueEventArgs<string>(message));
        }

        protected virtual void OnNotify(ValueEventArgs<string> e)
        {
            Notify?.Invoke(this, e);
        }
    }
}
