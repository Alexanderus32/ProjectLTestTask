using Core.NamedPipes;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ProjectLTestTask.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace ProjectLTestTask.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private Volume volume;

        private ClientObserver clientObserver;
        private IpcClient<string> client;
        private IDisposable clientWorked;

        private ObservableCollection<string> logs;

        public MainViewModel()
        {
            this.logs = new ObservableCollection<string>();
            CreateClient();
            ApplyCurrentVolumeCommand = new RelayCommand(ApplyCurrentVolumeMethod);
        }

        private void CreateClient()
        {
            this.clientObserver = new ClientObserver();
            this.clientObserver.ChangeVolume += SetVolume;
            this.clientObserver.Notify += LogMessage;
            this.client = new IpcClient<string>(".", "test", clientObserver);
            this.clientWorked = client.Create();
        }
        public ICommand ApplyCurrentVolumeCommand { get; private set; }

        public Volume Volume
        {
            get
            {
                if (this.volume == null)
                {
                    this.volume = new Volume();
                }
                return this.volume;
            }
            set
            {
                this.volume = value;
                RaisePropertyChanged("Volume");
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
            this.client.Observer.SetCurrentVolume(this.volume.LocalValue);
            SetVolume(this.volume.LocalValue);
        }

        private void SetVolume(int value)
        {
            this.volume.CurrentValue = value;
            this.volume.LocalValue = value;
        }

        private void LogMessage(string message)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                this.logs.Add(message);
            });           
        }
    }
}