using DownloadsManager.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Core.Concrete
{
    /// <summary>
    /// protocol provider for downloading
    /// </summary>
    [Serializable]
    public class HttpProtocolProvider : IProtocolProvider
    {
        /// <summary>
        /// creating response stream
        /// </summary>
        /// <param name="resourceInfo">downloading link</param>
        /// <param name="startRangePosition">range start position</param>
        /// <param name="endRangePosition">range end position</param>
        /// <returns></returns>
        public Stream CreateResponseStream(ResourceInfo resourceInfo, int startRangePosition, int endRangePosition)
        {
            if (resourceInfo != null)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(resourceInfo.Url);
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
            else
                return null;
        }

        /// <summary>
        /// Method for getting file information
        /// </summary>
        /// <param name="resourceInfo">link</param>
        /// <param name="stream">download stream</param>
        /// <returns>information about downloading file</returns>
        public RemoteFileInfo GetFileInfo(ResourceInfo resourceInfo, out Stream stream)
        {
            if (resourceInfo != null)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(resourceInfo.Url);
                request.Timeout = 30000;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                RemoteFileInfo result = new RemoteFileInfo();
                result.LastModified = response.LastModified;
                result.FileSize = response.ContentLength;
                result.AcceptRanges = string.Compare(response.Headers["Accept-Ranges"], "bytes", true, CultureInfo.CurrentCulture) == 0;

                stream = response.GetResponseStream();

                return result;
            }
            else
            {
                stream = null;
                return null;
            }
        }
    }
}
