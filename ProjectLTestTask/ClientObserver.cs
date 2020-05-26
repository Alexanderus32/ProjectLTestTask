﻿using Core.NamedPipes;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLTestTask
{
    class ClientObserver : IPipeStreamObserver<string>
    {
        public delegate void VolumeHandler(int volume);
        public event VolumeHandler ChangeVolume;

        public delegate void NotifyHandler(string value);
        public event NotifyHandler Notify;

        public void OnNext(string value)
        {
            bool isVolume = int.TryParse(value, out int volume);
            if(isVolume)
            {
                ChangeVolume?.Invoke(volume);
            }
            value = "Server: " + value;
            Task.Run(() => Notify?.Invoke(value));
        }

        public void OnError(Exception error)
        {
        }

        public void OnCompleted()
        {
            PipeStream.Write("Completed");
        }

        public PipeStream PipeStream { get; set; }

        public void OnConnected()
        {
            PipeStream.Write("Connected");
        }

        public void Say(string value)
        {
            PipeStream.Write(value);
        }

        public void GetCurrentVolume()
        {
            PipeStream.Write(CommandConstants.GetVolume.ToString());
        }

        public void SetCurrentVolume(int value)
        {
            PipeStream.Write(value.ToString());
        }

    }
}
