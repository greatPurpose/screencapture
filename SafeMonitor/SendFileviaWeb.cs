using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SafeMonitor
{
    public class SendFileviaWeb
    {
        public SendFileviaWeb(string filePath)
        {
            m_strfilepath = filePath;
        }

        public bool SendFile()
        {
            bool bln_return = false;

            if (File.Exists(m_strfilepath))
            {
                HttpClient client = new HttpClient();

                MultipartFormDataContent form = new MultipartFormDataContent();
                HttpContent content = new StringContent("fileToUpload");
                form.Add(content, "fileToUpload");
                var stream = new FileStream(m_strfilepath, FileMode.Open);
                content = new StreamContent(stream);
                var fileName = content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "screenfile",
                    FileName = Path.GetFileName(m_strfilepath),
                };
                form.Add(content);
                HttpResponseMessage response = null;

                var url = new Uri(Common.ServerEndpoint + "?task=upload&cname=" + this.GetComputerName());
                response = (client.PostAsync(url, form)).Result;
                
                if (response.IsSuccessStatusCode) bln_return = true;
            }

            return bln_return;
        }

        private string GetComputerName()
        {
            return System.Environment.MachineName;
        }

        private string m_strfilepath;
    }
}
