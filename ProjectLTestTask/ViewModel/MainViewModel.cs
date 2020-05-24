using Core;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ProjectLTestTask.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ProjectLTestTask.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private Volume currentVolume;
        private IpcClient<string> client;
        private IDisposable client1Worked;

        private ObservableCollection<string> logs;

        public MainViewModel()
        {
            client = new IpcClient<string>(".", "test", new ClientObserver());
            client1Worked = client.Create();
            logs = new ObservableCollection<string>();
            ApplyCurrentVolumeCommand = new RelayCommand(ApplyCurrentVolumeMethod);
        }
        public ICommand ApplyCurrentVolumeCommand { get; private set; }

        public Volume CurrentVolume
        {
            get
            {
                if (this.currentVolume == null)
                {
                    GetCurrentVolume();
                }
                return this.currentVolume;
            }
            set
            {
                this.currentVolume = value;
                RaisePropertyChanged("CurrentVolume");
            }
        }

        public ObservableCollection<string> Logs
        {
            get
            {
                return this.logs;
            }
        }
        private void ApplyCurrentVolumeMethod()
        {
            client.Observer.Say(currentVolume.Value.ToString());
            logs.Add("Apply volume");
            //Messenger.Default.Send<NotificationMessage>(new NotificationMessage("Apply volume"));
        }

        private void GetCurrentVolume()
        {
            //Todo get
            currentVolume = new Volume();
            logs.Add("Get volume");
            //Messenger.Default.Send<NotificationMessage>(new NotificationMessage("Get volume"));
        }
    }
}