using DownloadsManager.Core.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Core.Concrete
{
    public class HttpProtocolProvider : IProtocolProvider
    {
        public Stream CreateResponseStream(ResourceInfo ri, int startRangePosition, int endRangePosition)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ri.URL);
            request.Timeout = 30000;

            if (startRangePosition != 0)
            {
                if (endRangePosition == 0)
                {
                    request.AddRange(startRangePosition);
                }
                else
                {
                    request.AddRange(startRangePosition, endRangePosition);
                }
            }

            WebResponse response = request.GetResponse();

            return response.GetResponseStream();
        }

        public RemoteFileInfo GetFileInfo(ResourceInfo rl, out Stream stream)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(rl.URL);
            request.Timeout = 30000;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            RemoteFileInfo result = new RemoteFileInfo();
            result.LastModified = response.LastModified;
            result.FileSize = response.ContentLength;
            result.AcceptRanges = string.Compare(response.Headers["Accept-Ranges"], "bytes", true) == 0;
           
            stream = response.GetResponseStream();

            return result;
        }
    }
}
