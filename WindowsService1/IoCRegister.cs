using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsService1.Interfaces;

namespace WindowsService1
{
    class IoCRegister
    {
        public static readonly Container Container;

        static IoCRegister()
        {
            Container = new Container();
        }
        public static void Register()
        {
            Container.Register<ICommander, Commander>(Lifestyle.Singleton);
            Container.Register<IVolumeService, VolumeService>(Lifestyle.Singleton);
            Container.Register<ServerObserver>(Lifestyle.Singleton);
            Container.Verify();
        }
    }
}
