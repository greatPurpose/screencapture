using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.Timers;

namespace StartService
{
    public partial class Service1 : ServiceBase
    {
        System.Timers.Timer timer = new System.Timers.Timer();
        public Service1()
        {
            InitializeComponent();
            
        }
        internal void StartMonitor(string[] args)
        {
            this.OnStart(args);
            while (true)
            {
                Thread.Sleep(60 * 1000);
            }
        }

        protected override void OnStart(string[] args)
        {
            monitorfile = AppDomain.CurrentDomain.BaseDirectory + "\\SafeMonitor.exe";
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = Setting.interval; //number in milisecinds  
            timer.Enabled = true;


        }

        protected override void OnStop()
        {
        }

        public void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            System.Diagnostics.Process[] proc = System.Diagnostics.Process.GetProcessesByName("SafeMonitor");
            
            if (proc.Length < 1)
            {
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo(monitorfile);
                process.Start();
            }
        }

        private string monitorfile = "";
    }
}
