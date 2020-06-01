using Core.NamedPipes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectLTestTask
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ClientObserver clientObserver;
        private IpcClient<string> client;
        private IDisposable clientWorked;

        public App()
        {
            CreateClient();
        }

        private void CreateClient()
        {
            clientObserver = new ClientObserver();
            this.client = new IpcClient<string>(".", "test", clientObserver);
            this.clientWorked = client.Create();
        }
    }
}
