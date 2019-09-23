using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMonitor
{
    public class Common
    {
        public static int MainThreadInterval = 60 * 1000; //60s
        public static int CaptureInterval = 600 * 1000; //60s
        public static int ProcessMonitorInterval = 10 * 1000; //60s
        public static string ServerEndpoint = "http://www.workspace.com/screenview/webapi.php";
    }
}
