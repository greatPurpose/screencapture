using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace LuanchApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string strCmdText;
            strCmdText = "StartService.exe";
            System.Diagnostics.Process.Start("InstallUtil.exe", strCmdText);            
            
            
            ServiceController sc = new ServiceController("StartService");
            sc.Start();
            sc.WaitForStatus(ServiceControllerStatus.Running);
            

        }
    }
}
