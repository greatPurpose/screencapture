using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Net;
using System.Drawing.Imaging;

namespace SafeMonitor
{
    public class ScreenCapture
    {
        public ScreenCapture()
        { }

        public void ScreenShot()
        {

            m_strDir = AppDomain.CurrentDomain.BaseDirectory + "\\safe";
            if (!Directory.Exists(m_strDir))
                Directory.CreateDirectory(m_strDir);

            DateTime dt = DateTime.UtcNow;
            string local = GetComputerName();
            string screenName = m_strDir + "\\" + local + "_" + dt.ToString("yyyyMMdd_HHmmss");
            Rectangle scr = new Rectangle();
            int x = 0, y = 0;

            for (int i = 0;  i < System.Windows.Forms.Screen.AllScreens.Count(); i ++)
            {
                scr.Width += Screen.AllScreens[i].WorkingArea.Width;
                if (scr.Height < Screen.AllScreens[i].WorkingArea.Height)
                    scr.Height = Screen.AllScreens[i].WorkingArea.Height;
                if (x > Screen.AllScreens[i].WorkingArea.X)
                    x = Screen.AllScreens[i].WorkingArea.X;
                if (y > Screen.AllScreens[i].WorkingArea.Y)
                    y = Screen.AllScreens[i].WorkingArea.Y;
            }
            if (!scr.IsEmpty)
            {
                Bitmap bmp = new Bitmap(scr.Width, scr.Height);
                Graphics graph = Graphics.FromImage(bmp as Image);
                graph.CopyFromScreen(x, y, 0, 0, bmp.Size);
                EncoderParameters encoder = new EncoderParameters(1);
                encoder.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 50L);
                bmp.Save(screenName, GetEncoder(ImageFormat.Jpeg), encoder);
            }            
        }
        public string CaptureDirectory
        {         
            get
            {
                return m_strDir;
            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        private string GetComputerName()
        {
            return System.Environment.MachineName;
        }

        private string m_strDir;
    }
}
