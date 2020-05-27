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
        private IDisposable client1Worked;

        private ObservableCollection<string> logs;

        public MainViewModel()
        {
            Thread.Sleep(10000);
            logs = new ObservableCollection<string>();
            clientObserver = new ClientObserver();
            clientObserver.ChangeVolume += SetVolume;
            clientObserver.Notify += LogMessage;
            client = new IpcClient<string>(".", "test", clientObserver);

            client1Worked = client.Create();
            ApplyCurrentVolumeCommand = new RelayCommand(ApplyCurrentVolumeMethod);
        }
        public ICommand ApplyCurrentVolumeCommand { get; private set; }

        public Volume Volume
        {
            get
            {
                if (this.volume == null)
                {
                    GetCurrentVolume();
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
            client.Observer.SetCurrentVolume(this.volume.LocalValue);
            SetVolume(this.volume.LocalValue);
        }

        private void GetCurrentVolume()
        {
            volume = new Volume();
            client.Observer.GetCurrentVolume();
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
                logs.Add(message);
            });
            
        }
    }
}