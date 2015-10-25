using DownloadsManager.Core.Abstract;
using DownloadsManager.Core.Concrete;
using DownloadsManager.Core.Concrete.DownloadStates;
using DownloadsManager.Core.Concrete.Enums;
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
    public class MainWindowVM : MainVM
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

        public List<DownloadStatisticWrapper> DownloadTypesStatistic 
        { 
            get
            {
                return GetTypesStatistic();
            }
        }

        public List<DownloadStatisticWrapper> DownloadStatesStatistic
        {
            get
            {
                return GetStatesStatistic();
            }
        }

        public long TotalBytesDownloadedStatistic
        { 
            get
            {
                return _itemsToDownloaders.Keys.OfType<Downloader>().Select(d => d.FileSize).Sum();
            }
        }

        private void AddSavedDownloads()
        {
            try
            {
                var downloads = DownloadsSerializer.Deserialize();
                foreach (var fileToDownload in downloads)
                {
                    //@
                    if (fileToDownload.State.State != DownloadState.Ended
                        && fileToDownload.State.State != DownloadState.EndedWithError)
                    {
                        DownloaderManager.Instance.Add(fileToDownload, true);
                    }
                    var viewer = new DownloadViewer();
                    viewer.DataContext = new DownloadViewerVM(fileToDownload);
                    _itemsToDownloaders.Add(fileToDownload, viewer);
                }

                NotifyPropertyChanged("ItemsToDownloaders");
            }
            catch (FileNotFoundException)
            {
                //@
            }
        }

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

        /// <summary>
        /// Command for closing app window and saving results
        /// </summary>
        public Command CloseCmd { get; set; }

        private void Close(object param)
        {
            List<Downloader> downloadsToSave = new List<Downloader>();
            foreach(var download in _itemsToDownloaders.Keys.OfType<Downloader>().ToList())
            {
                try
                {
                    download.State.Pause();
                }
                catch(InvalidOperationException)
                {
                    Console.WriteLine();
                }
                downloadsToSave.Add(download);
            }
            DownloadsSerializer.Serialize(downloadsToSave);
        }

        //AutoFac
        /// <summary>
        /// Method for adding download
        /// </summary>
        /// <param name="param">Download param</param>
        private void AddDownload(object param)
        {
            NewDownloadView newDownloadView = new NewDownloadView();
            newDownloadView.ShowDialog();
            string fileName = string.Empty;
            fileName = newDownloadView.Model.Mirror != null
                ? GetFileName(newDownloadView.Model.Mirror)
                : GetFileName(newDownloadView.Model.Mirrors.First());

            Downloader fileToDownload = new Downloader(
                newDownloadView.Model.Mirror, 
                newDownloadView.Model.Mirrors.ToArray(),
                newDownloadView.Model.SavePath, 
                fileName);
            DownloaderManager.Instance.Add(fileToDownload, true);
                    
            var viewer = new DownloadViewer();
            
            viewer.DataContext = new DownloadViewerVM(fileToDownload);

            _itemsToDownloaders.Add(fileToDownload, viewer);
            NotifyPropertyChanged("ItemsToDownloaders");
        }

        private static string GetFileName(ResourceInfo mirror)
        {
            Uri uri = new Uri(mirror.Url);
            var fileName = uri.Segments[uri.Segments.Length - 1];
            return HttpUtility.UrlDecode(fileName).Replace("/", "\\");
        }

        #endregion

        #region Statistic
        private List<DownloadStatisticWrapper> GetTypesStatistic()
        {
            List<DownloadStatisticWrapper> result = new List<DownloadStatisticWrapper>();
            foreach(Downloader file in _itemsToDownloaders.Keys)
            {
                if (result.Where(d => d.FileType == file.FileType).Count() > 0)
                    result.Where(d => d.FileType == file.FileType).FirstOrDefault().Count++;
                else
                    result.Add(new DownloadStatisticWrapper(file.FileType,1,file.State.State));
            }
            return result;
        }

        private List<DownloadStatisticWrapper> GetStatesStatistic()
        {
            List<DownloadStatisticWrapper> result = new List<DownloadStatisticWrapper>();
            foreach (Downloader file in _itemsToDownloaders.Keys)
            {
                if (result.Where(d => d.State == file.State.State).Count() > 0)
                    result.Where(d => d.State == file.State.State).FirstOrDefault().Count++;
                else
                    result.Add(new DownloadStatisticWrapper(file.FileType, 1, file.State.State));
            }
            return result;
        }
        #endregion
    }
}
