﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLTestTask
{
    public class ValueEventArgs<T> : EventArgs
    {
        public T Value { get; set; }

        public ValueEventArgs(T value)
        {
            this.Value = value;
        }
    }
}
