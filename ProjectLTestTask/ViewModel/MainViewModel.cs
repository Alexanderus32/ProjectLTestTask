using CommonServiceLocator;
using Core;
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

        private IClientOberver client;

        public MainViewModel()
        {
            this.client = ServiceLocator.Current.GetInstance<IClientOberver>();
            this.client.ChangeVolume += new EventHandler<ValueEventArgs<int>>(SetVolume);
            this.client.Notify += new EventHandler<ValueEventArgs<string>>(LogMessage);

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
            client.VolumeChangeHandler(this.volume.LocalValue);
            SetVolume(null, new ValueEventArgs<int>(this.volume.LocalValue));
        }

        private void SetVolume(object source, ValueEventArgs<int> args)
        {
            this.Volume = new Volume { CurrentValue = args.Value, LocalValue = args.Value };
        }

        private void LogMessage(object source, ValueEventArgs<string> args)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                this.logs.Add(args.Value);
            });           
        }
    }
}