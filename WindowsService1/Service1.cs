using Core;
using Core.NamedPipes;
using System;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
        }

        protected override void OnStart(string[] args)
        {
            var server = new IpcServer<string>("test");
            server.Start<ServerObserver>();

        }

        protected override void OnStop()
        {
        }
    }
}
