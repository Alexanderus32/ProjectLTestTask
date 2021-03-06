﻿using Core;
using Core.NamedPipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLTestTask
{
    interface IClientOberver : IPipeStreamObserver<string>
    {
        event EventHandler<ValueEventArgs<int>> ChangeVolume;

        event EventHandler<ValueEventArgs<string>> Notify;

        void VolumeChangeHandler(int value);

        void Say(string value);

    }
}
