using DownloadsManager.Core.Concrete;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.ViewModels
{
    /// <summary>
    /// View model for DownloadViewer
    /// </summary>
    public class DownloadViewerVM
    {
        private Downloader download;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="download"> downoad to v</param>
        public DownloadViewerVM(Downloader download)
        {
            this.download = download;
        }

        /// <summary>
        /// Size info of download
        /// </summary>
        public string SizeInfo 
        { 
            get
            {
                return download.Transfered + "from" + download.FileSize;
            }
        }

        /// <summary>
        /// State of download
        /// </summary>
        public string State 
        { 
            get
            {
                return download.State.ToString();
            }
        }

        /// <summary>
        /// Added time for download
        /// </summary>
        public string Added 
        { 
            get
            {
                return download.CreatedDateTime.ToString();
            }
        }

        /// <summary>
        /// Rate of download
        /// </summary>
        public string Rate 
        { 
            get
            {
                return download.Rate.ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Link info about download
        /// </summary>
        public string LinkInfo 
        { 
            get
            {
                return download.ResourceInfo.URL.ToString();
            }
        }

        /// <summary>
        /// Progress of download
        /// </summary>
        public string Progress 
        { 
            get
            {
                return download.Progress.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}
