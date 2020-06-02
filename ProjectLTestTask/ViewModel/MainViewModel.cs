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

        private ObservableCollection<string> logs;

        public MainViewModel()
        {
            App.clientObserver.ChangeVolume += SetVolume;
            App.clientObserver.Notify += LogMessage;
            App.client.Create();
            this.logs = new ObservableCollection<string>();
            ApplyCurrentVolumeCommand = new RelayCommand(ApplyCurrentVolumeMethod);
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
                if (this.volume == null)
                {
                    this.volume = new Volume();
                }
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
            App.clientObserver.VolumeChangeHandler(this.volume.LocalValue);
            SetVolume(this.volume.LocalValue);
        }

        private void SetVolume(int value)
        {
            this.Volume = new Volume { CurrentValue = value, LocalValue = value };
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