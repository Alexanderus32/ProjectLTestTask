
using CommonServiceLocator;
using Core.NamedPipes;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;

namespace ProjectLTestTask.ViewModel
{
   
    public class ViewModelLocator
    {
        public static ClientObserver clientObserver;
        private IpcClient<string> client;

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainViewModel>();
            
            clientObserver = new ClientObserver();
            client = new IpcClient<string>(".", "test", clientObserver);
            client.Create();
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
            SimpleIoc.Default.Unregister<MainViewModel>();
        }
    }
}