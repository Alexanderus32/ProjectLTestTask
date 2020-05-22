using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLTestTask.Models
{
    public class Volume : ObservableObject
    {
        private int value = 10;

        public int Value
        {
            get
            {
                return this.value;
            }
            set
            {
                Set<int>(() => this.Value, ref this.value, value);
            }
        }

    }
}
