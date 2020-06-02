using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.NamedPipes
{
    public interface IIterator<out T>
    {
        IObservable<T> ReadNext();
    }
}
