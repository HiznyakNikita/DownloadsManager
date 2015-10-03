using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Core.Abstract
{
    /// <summary>
    /// interface for dowmnloader
    /// </summary>
   public interface IDownloader
    {
       /// <summary>
       /// for downloading
       /// </summary>
       /// <param name="uri">uri</param>
       /// <param name="path">path to download</param>
        void Download(string uri, string path);
    }
}
