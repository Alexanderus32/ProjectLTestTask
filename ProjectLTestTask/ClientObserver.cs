using Core;
using Core.NamedPipes;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLTestTask
{
    public class ClientObserver : IClientOberver
    {

        public PipeStream PipeStream { get; set; }

        public event EventHandler<ValueEventArgs<int>> ChangeVolume;

        public event EventHandler<ValueEventArgs<string>> Notify;

        protected virtual void OnChangeVolume(ValueEventArgs<int> e)
        {
            ChangeVolume?.Invoke(this, e);
        }

        protected virtual void OnNotify(ValueEventArgs<string> e)
        {
            Notify?.Invoke(this, e);
        }


        public void OnNext(string value)
        {
            bool isVolume = int.TryParse(value, out int volume);
            if (isVolume)
            {
                OnChangeVolume(new ValueEventArgs<int>(volume));
            }
            value = "Server: " + value;
            Task.Run(() => OnNotify(new ValueEventArgs<string>(value)));
        }

        public void OnError(Exception error)
        {

        }

        public void OnCompleted()
        {
            PipeStream.Write("Completed");
        }       

        public void OnConnected()
        {
            PipeStream.Write("Connected");
        }

        public void Say(string value)
        {
            PipeStream.Write(value);
        }

        public void VolumeChangeHandler(int value)
        {
            PipeStream.Write(value.ToString());
        }

    }
}
