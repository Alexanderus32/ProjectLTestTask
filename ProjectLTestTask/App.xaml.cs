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
        public static IpcClient<string> client;

        public App()
        {
            CreateClient();
        }

        private void CreateClient()
        {
            clientObserver = new ClientObserver();
            client = new IpcClient<string>(".", "test", clientObserver);
        }

    }
}
