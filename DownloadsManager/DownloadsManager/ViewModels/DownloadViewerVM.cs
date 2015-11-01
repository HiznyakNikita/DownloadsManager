using DownloadsManager.Core.Concrete;
using DownloadsManager.Helpers;
using DownloadsManager.ViewModels.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.ViewModels
{
    
    /// <summary>
    /// View model for DownloadViewer
    /// </summary>
    public class DownloadViewerVM : MainVM, IDownloadViewerVM
    {
        private Downloader download;

        /// <summary>
        /// ctor    
        /// </summary>
        /// <param name="downloader">download for view</param>
        public DownloadViewerVM(Downloader downloader)
        {
            try
            {
                if (downloader != null)
                    this.download = downloader;

                this.StartDownloadCmd = new Command(this.StartDownload);
                this.PauseDownloadCommand = new Command(this.PauseDownload);
                this.ShowInFolderCmd = new Command(this.ShowInFolder);
                this.DownloadSettingsCmd = new Command(this.DownloadSettings);
                //// Attach EventHandler
                this.download.StateChanged += download_StateChanged;
                this.download.ThreadAdded += download_ThreadAdded;
                this.download.SegmentChanged += download_SegmentChanged;
                //this.download.LocalFileInfoChanged += download_LocalFileInfoChanged;
                //this.download.RemoteFileInfoChanged += download_RemoteFileInfoChanged;
            }
            catch (NullReferenceException)
            {
            }
        }

        /// <summary>
        /// Gets download
        /// </summary>
        public Downloader Download 
        { 
            get
            {
                return download;
            }
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
        /// Gets size info of download
        /// </summary>
        public string SizeInfo 
        { 
            get
            {
                return download.TransferBytes/1000 + " kb from " + download.FileSize/1000 + " kb";
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
                return "Added: " + download.CreatedDateTime.ToString();
            }
        }

        /// <summary>
        /// Rate of download
        /// </summary>
        public string Rate 
        { 
            get
            {
                return (download.Rate / 100).ToString(CultureInfo.InvariantCulture) != "0" ? 
                    (download.Rate / 100).ToString(CultureInfo.InvariantCulture).Substring(0,4) + " kb/sec" :
                    "0 kb/sec";
            }
        }

        /// <summary>
        /// Link info about download
        /// </summary>
        public string LinkInfo 
        { 
            get
            {
                return "Url: " + download.ResourceInfo.Url.ToString();
            }
        }

        /// <summary>
        /// Progress of download
        /// </summary>
        public string Progress 
        { 
            get
            {
                return download.Progress.ToString(CultureInfo.InvariantCulture).Length > 4 ? 
                    download.Progress.ToString(CultureInfo.InvariantCulture).Substring(0, 4) + " %" : "0.0 %";
            }
        }

        public Command PauseDownloadCommand { get; set; }

        public Command StartDownloadCmd { get; set; }

        public Command DownloadSettingsCmd { get; set; }
        /// <summary>
        /// Command for showing download in folder
        /// </summary>
        public Command ShowInFolderCmd { get; set; }

        private void DownloadSettings(object param)
        {
            try
            {
                if(!string.IsNullOrEmpty(param.ToString()))
                {
                    download.MaxRate = Convert.ToDouble(param.ToString(), new NumberFormatInfo());
                }
            }
            catch(Exception)
            {
                
            }
        }

        private void ShowInFolder(object param)
        {
            Process.Start(download.LocalFile);
        }

        private void StartDownload()
        {
            try
            {
                download.Start();
            }
            catch(InvalidOperationException) 
            { 
            }
        }

        private void PauseDownload()
        {
            try
            {
                download.Pause();
            }
            catch (InvalidOperationException) 
            { 
            }
        }

        //void download_RemoteFileInfoChanged(object sender, EventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        //void download_LocalFileInfoChanged(object sender, EventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        void download_SegmentChanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged("Progress");
            NotifyPropertyChanged("Rate");
            NotifyPropertyChanged("SizeInfo");
        }

        void download_StateChanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged("State");
        }

        void download_ThreadAdded(object sender, EventArgs e)
        {
            NotifyPropertyChanged("Progress");
            NotifyPropertyChanged("Rate");
            NotifyPropertyChanged("SizeInfo");
        }
    }
}
