using System;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;

namespace StartService
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new System.Timers.Timer();
        public Service1()
        {
            InitializeComponent();
            
        }

        protected override void OnStart(string[] args)
        {
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = Setting.interval; //number in milisecinds  
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
        }

        public void OnElapsedTime(object source, ElapsedEventArgs e)
        {                      
            ServiceController sc = new ServiceController(Setting.mointorname);            
            if (sc.Status == ServiceControllerStatus.Stopped)
            {
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running);
            }
        }
    }
}
