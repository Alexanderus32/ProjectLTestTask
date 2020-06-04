using Core.Interfaces;
using SimpleInjector;

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
            Container.Register<ISend, Sender>(Lifestyle.Singleton);
            Container.Verify();
        }
    }
}
