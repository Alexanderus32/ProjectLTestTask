using CommonServiceLocator;
using Core;
using Core.Interfaces;
using ProjectLTestTask.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectLTestTask
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
            ViewModelLocator.Cleanup();
        }

        protected override void OnClosed(EventArgs e)
        {
            Dispose();
            base.OnClosed(e);
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            var dictionary = new Dictionary<string, string> { { "Command", CommandConstants.GetVolume.ToString() } };
            string json = MessageConverter.CreateJson(new AudioMessage(), dictionary);
            ServiceLocator.Current.GetInstance<ISend>().Send(json);
        }
    }
}
