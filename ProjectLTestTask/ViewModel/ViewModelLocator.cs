
using CommonServiceLocator;
using Core.NamedPipes;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using ProjectLTestTask.Services;
using System;
using System.Windows;
using WindowsService1;
using WindowsService1.Interfaces;

namespace ProjectLTestTask.ViewModel
{
   
    public class ViewModelLocator
    {
        private static IDisposable clientWorked;

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<IClientOberver, ClientObserver>();

            SimpleIoc.Default.Register<ICommander, ClientCommander>();
            SimpleIoc.Default.Register<IVolumeService, ClientAudioService>();
            SimpleIoc.Default.Register<ISend, Sender>();

            var client = new IpcClient<string>(".", "test", ServiceLocator.Current.GetInstance<IClientOberver>());
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
            SimpleIoc.Default.Unregister<IClientOberver>();
            SimpleIoc.Default.Unregister<MainViewModel>();
        }
    }
}