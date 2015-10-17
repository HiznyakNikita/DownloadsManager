using DownloadsManager.Core.Concrete;
using DownloadsManager.Helpers;
using DownloadsManager.UserControls;
using DownloadsManager.ViewModels.Infrastructure;
using DownloadsManager.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Controls;
using System.Windows.Input;

namespace DownloadsManager.ViewModels
{
    /// <summary>
    /// Main Window View Model
    /// </summary>
    public class MainWindowVM : INotifyPropertyChanged
    {
        private Hashtable _itemsToDownloaders = new Hashtable();

        /// <summary>
        /// ctor
        /// </summary>
        public MainWindowVM()
        {
            this.AddDownloadCmd = new Command(this.AddDownload);
            this.CloseCmd = new Command(this.Close);
            AddSavedDownloads();

        }

        private void AddSavedDownloads()
        {
            try
            {
                List<Downloader> downloads = DownloadsSerializer.Deserialize();
                foreach (var fileToDownload in downloads)
                {
                    DownloaderManager.Instance.Add(fileToDownload, true);
                    DownloadViewer viewer = new DownloadViewer();
                    viewer.DataContext = new DownloadViewerVM(fileToDownload);
                    _itemsToDownloaders.Add(fileToDownload, viewer);
                }

                NotifyPropertyChanged("ItemsToDownloaders");
            }
            catch (FileNotFoundException)
            {

            }
        }
            

        public event PropertyChangedEventHandler PropertyChanged;

        public Hashtable ItemsToDownloaders
        { 
            get
            {
                return _itemsToDownloaders;
            }
        }

        #region Commands

        /// <summary>
        /// Gets or sets Command for adding download
        /// </summary>
        public Command AddDownloadCmd { get; set; }

        public Command CloseCmd { get; set; }

        private void Close(object param)
        {
            List<Downloader> downloadsToSave = new List<Downloader>();
            foreach(var download in _itemsToDownloaders.Keys.OfType<Downloader>().ToList())
            {
                download.State.Pause();
                downloadsToSave.Add(download);
            }
            DownloadsSerializer.Serialize(downloadsToSave);
        }

        /// <summary>
        /// Method for adding download
        /// </summary>
        /// <param name="param">Download param</param>
        private void AddDownload(object param)
        {
            NewDownloadView newDownloadView = new NewDownloadView();
            newDownloadView.ShowDialog();
            string fileName = string.Empty;
            if (newDownloadView.Model.Mirror != null)
            {
                Uri uri = new Uri(newDownloadView.Model.Mirror.Url);
                fileName = uri.Segments[uri.Segments.Length - 1];
                fileName = HttpUtility.UrlDecode(fileName).Replace("/", "\\");
            }
            else if (newDownloadView.Model.Mirrors.Count != 0)
            {
                Uri uri = new Uri(newDownloadView.Model.Mirrors[0].Url);
                fileName = uri.Segments[uri.Segments.Length - 1];
                fileName = HttpUtility.UrlDecode(fileName).Replace("/", "\\");
            }

            Downloader fileToDownload = new Downloader(
                newDownloadView.Model.Mirror, 
                newDownloadView.Model.Mirrors.ToArray(),
                newDownloadView.Model.SavePath, 
                newDownloadView.Model.SegmentsCount, 
                fileName);
            DownloaderManager.Instance.Add(fileToDownload, true);
                    
            DownloadViewer viewer = new DownloadViewer();
            
            viewer.DataContext = new DownloadViewerVM(fileToDownload);

            _itemsToDownloaders.Add(fileToDownload, viewer);
            NotifyPropertyChanged("ItemsToDownloaders");
        }

        #endregion

        #region INotifyPropertyChanged

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
