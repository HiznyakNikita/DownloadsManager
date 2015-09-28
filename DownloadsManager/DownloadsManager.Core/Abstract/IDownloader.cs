using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Core.Abstract
{
    interface IDownloader
    {
        void Download(string uri, string path);
    }
}
