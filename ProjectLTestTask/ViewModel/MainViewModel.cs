using CommonServiceLocator;
using Core;
using Core.Interfaces;
using Core.NamedPipes;
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
        private Volume volume;

        private ObservableCollection<string> logs;

        private IVolumeService volumeService;
        private INotificator notificator;

        public MainViewModel(IVolumeService service, INotificator notificator)
        {
            this.volumeService = service;
            this.notificator = notificator;
            this.volumeService.ChangeVolume += new EventHandler<ValueEventArgs<int>>(SetVolume);
            this.notificator.Notify += new EventHandler<ValueEventArgs<string>>(LogMessage);

            this.logs = new ObservableCollection<string>();
            ApplyCurrentVolumeCommand = new RelayCommand(ApplyCurrentVolumeMethod);
        }
     
        public System.Windows.Input.ICommand ApplyCurrentVolumeCommand { get; private set; }

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
            volumeService.SetVolume(this.volume.LocalValue);
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