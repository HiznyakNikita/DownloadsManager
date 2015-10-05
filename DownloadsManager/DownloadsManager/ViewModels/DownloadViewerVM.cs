using DownloadsManager.Core.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.ViewModels
{
    public class DownloadViewerVM
    {
        private Downloader download;

        public DownloadViewerVM(Downloader download)
        {
            this.download = download;
        }

        public string SizeInfo { get; set; }
        public string State { get; set; }
        public string Added { get; set; }
        public string Rate { get; set; }
        public string UrlInfo { get; set; }
        public string Progress { get; set; }
    }
}
