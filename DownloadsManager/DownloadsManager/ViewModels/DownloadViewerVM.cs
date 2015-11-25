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
        private readonly Dictionary<DateTime, double> _ratesStatistic = new Dictionary<DateTime, double>();
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

                this.StartDownloadCommand = new Command(this.StartDownload);
                this.PauseDownloadCommand = new Command(this.PauseDownload);
                this.ShowInFolderCommand = new Command(this.ShowInFolder);
                this.RemoveCommand = new Command(this.RemoveDownload);
                this.DownloadSettingsCommand = new Command(this.DownloadSettings);
                //// Attach EventHandler
                if (download != null)
                {
                    this.download.StateChanged += Download_StateChanged;
                    this.download.ThreadAdded += Download_ThreadAdded;
                    this.download.SegmentChanged += Download_SegmentChanged;
                }
                //this.download.LocalFileInfoChanged += download_LocalFileInfoChanged;
                //this.download.RemoteFileInfoChanged += download_RemoteFileInfoChanged;
            }
            catch (NullReferenceException)
            {
            }
        }

        /// <summary>
        /// Rate statistic dictionary -> dateTime of download - rate of download
        /// </summary>
        public Dictionary<DateTime, double> RatesStatistic
        {
            get
            {
                return _ratesStatistic;
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
                if (download != null)
                    return download.FileName;
                else
                    return string.Empty;
            }
        }

        /// <summary>
        /// Gets size info of download
        /// </summary>
        public string SizeInfo 
        { 
            get
            {
                if (download != null)
                    return ((download.TransferBytes / 1000) + " kb from ") + ((download.FileSize / 1000) + " kb");
                else
                    return string.Empty;
            }
        }

        /// <summary>
        /// State of download
        /// </summary>
        public string State 
        { 
            get
            {
                if (download != null)
                    return download.State.ToString();
                else
                    return string.Empty;
            }
        }

        /// <summary>
        /// Added time for download
        /// </summary>
        public string Added 
        { 
            get
            {
                if (download != null)
                    return "Added: " + download.CreatedDateTime.ToString();
                else
                    return string.Empty;
            }
        }

        /// <summary>
        /// Rate of download
        /// </summary>
        public string Rate 
        { 
            get
            {
                if (download != null)
                    return (download.Rate / 100).ToString(CultureInfo.InvariantCulture) != "0" ?
                    (download.Rate / 100).ToString(CultureInfo.InvariantCulture).Substring(0, 4) + " kb/sec" :
                    "0 kb/sec";
                else
                    return string.Empty;
            }
        }

        /// <summary>
        /// Link info about download
        /// </summary>
        public string LinkInfo 
        { 
            get
            {
                if (download != null)
                    return "Url: " + download.ResourceInfo.Url.ToString();
                else
                    return string.Empty;
            }
        }

        /// <summary>
        /// Progress of download
        /// </summary>
        public string Progress 
        { 
            get
            {
                if (download != null)
                    return Math.Round(download.Progress, 1).ToString(CultureInfo.CurrentCulture) + " %";
                else
                    return string.Empty;
            }
        }

        public Command PauseDownloadCommand { get; set; }

        public Command StartDownloadCommand { get; set; }

        /// <summary>
        /// Command for add max rate to download settings
        /// </summary>
        public Command DownloadSettingsCommand { get; set; }

        public Command RemoveCommand { get; set; }

        /// <summary>
        /// Command for showing download in folder
        /// </summary>
        public Command ShowInFolderCommand { get; set; }

        private void RemoveDownload(object param)
        {
            try
            {
                DownloaderManager.Instance.RemoveDownload(this.Download);
            }
            catch (Exception)
            {

            }
        }

        private void DownloadSettings(object param)
        {
            try
            {
                if (!string.IsNullOrEmpty(param.ToString()))
                {
                    download.MaxRate = Convert.ToDouble(param.ToString(), new NumberFormatInfo());
                }
            }
            catch (Exception)
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
            catch (InvalidOperationException) 
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

        private void Download_SegmentChanged(object sender, EventArgs e)
        {
            try
            {
                NotifyPropertyChanged("Progress");
                NotifyPropertyChanged("Rate");
                NotifyPropertyChanged("SizeInfo");
                //save rate statistic
                RatesStatistic.Add(DateTime.Now, download.Rate);
            }
            catch (ArgumentException)
            {

            }
        }

        private void Download_StateChanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged("State");
        }

        private void Download_ThreadAdded(object sender, EventArgs e)
        {
            NotifyPropertyChanged("Progress");
            NotifyPropertyChanged("Rate");
            NotifyPropertyChanged("SizeInfo");
        }
    }
}
