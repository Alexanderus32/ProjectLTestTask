
using CommonServiceLocator;
using Core.Interfaces;
using Core.NamedPipes;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using ProjectLTestTask.Services;
using System;
using WindowsService1;

namespace ProjectLTestTask.ViewModel
{
   
    public class ViewModelLocator
    {
        private static IDisposable clientWorked;

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<IPipeStreamObserver<string>, ClientObserver>();
            SimpleIoc.Default.Register<ICommander, ClientCommander>();
            SimpleIoc.Default.Register<IVolumeService, ClientAudioService>();
            SimpleIoc.Default.Register<ISend, Sender>();
            SimpleIoc.Default.Register<INotificator, Notificator>();

            var client = new IpcClient<string>(".", "test", ServiceLocator.Current.GetInstance<IPipeStreamObserver<string>>());
            clientWorked = client.Create();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public static void Cleanup()
        {
            clientWorked.Dispose();
            SimpleIoc.Default.Unregister<IPipeStreamObserver<string>>();
            SimpleIoc.Default.Unregister<MainViewModel>();
            SimpleIoc.Default.Unregister<ICommander>();
            SimpleIoc.Default.Unregister<IVolumeService>();
            SimpleIoc.Default.Unregister<ISend>();
            SimpleIoc.Default.Unregister<INotificator>();
        }
    }
}