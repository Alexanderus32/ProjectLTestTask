using Core;
using Core.NamedPipes;
using System;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService1
{
    public partial class VolumeControlService : ServiceBase
    {
        public VolumeControlService()
        {
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
        }

        protected override void OnStart(string[] args)
        {
            IoCRegister.Register();
            var server = new IpcServer<string>("test");
            server.Start<ServerObserver>();
        }

        public void OnStartDebug()
        {
            OnStart(null);
        }

        protected override void OnStop()
        {
        }
    }
}
