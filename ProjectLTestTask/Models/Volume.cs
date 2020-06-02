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
        private int currentValue;

        private int localValue;

        public int CurrentValue
        {
            get
            {
                return this.currentValue;
            }
            set
            {
                Set<int>(() => this.CurrentValue, ref this.currentValue, value);
            }
        }

        public int LocalValue
        {
            get
            {
                return this.localValue;
            }
            set
            {
                Set<int>(() => this.LocalValue, ref this.localValue, value);
            }
        }

    }
}
