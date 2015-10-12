using DownloadsManager.Core.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.ViewModels
{
    /// <summary>
    /// View model for DownloadViewer
    /// </summary>
    public class DownloadViewerVM : INotifyPropertyChanged
    {
        private Downloader download;

        /// <summary>
        /// ctor    
        /// </summary>
        /// <param name="downloader">download for view</param>
        public DownloadViewerVM(Downloader downloader)
        {
            if(downloader!=null)
                this.download = downloader;
            //// Attach EventHandler
            this.download.PropertyChanged += Downloader_PropertyChanged;
        }

        /// <summary>
        /// Gets download name
        /// </summary>
        public string FileName 
        { 
            get
            {
                return download.FileName;
            }
        }

        /// <summary>
        /// Size info of download
        /// </summary>
        public string SizeInfo 
        { 
            get
            {
                return download.Transfered + " bytes from " + download.FileSize + " bytes";
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
                return download.Progress.ToString(CultureInfo.InvariantCulture).Length > 4 ? download.Progress.ToString(CultureInfo.InvariantCulture).Substring(0,4) + " %" : "0.0 %";
            }
        }

        private void Downloader_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("Progress");
            NotifyPropertyChanged("LinkInfo");
            NotifyPropertyChanged("Rate");
            NotifyPropertyChanged("State");
            NotifyPropertyChanged("Info");
            NotifyPropertyChanged("SizeInfo");
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        #endregion
    }
}
