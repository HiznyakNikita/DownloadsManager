using DownloadsManager.Core.Concrete;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public string SizeInfo 
        { 
            get
            {
                return download.Transfered + "from" + download.FileSize;
            }
        }

        public string State 
        { 
            get
            {
                return download.State.ToString();
            }
        }

        public string Added 
        { 
            get
            {
                return download.CreatedDateTime.ToString();
            }
        }

        public string Rate 
        { 
            get
            {
                return download.Rate.ToString(CultureInfo.InvariantCulture);
            }
        }

        public string LinkInfo 
        { 
            get
            {
                return download.ResourceInfo.URL.ToString();
            }
        }

        public string Progress 
        { 
            get
            {
                return download.Progress.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}
