using DownloadsManager.Core.Abstract;
using DownloadsManager.Core.Concrete;
using DownloadsManager.Core.Concrete.DownloadStates;
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
        private Dictionary<Downloader, DownloadViewer> _itemsToDownloaders = new Dictionary<Downloader,DownloadViewer>();

        /// <summary>
        /// ctor
        /// </summary>
        public MainWindowVM()
        {
            this.AddDownloadCmd = new Command(this.AddDownload);
            this.CloseCmd = new Command(this.Close);
            AddSavedDownloads();

        }

        public List<DownloadViewer> MusicDownloads 
        { 
            get
            {
                List<Downloader> downloads =  _itemsToDownloaders.Keys.Where(d => d.FileType == Core.Concrete.Enums.FileType.Music).ToList();
                List<DownloadViewer> views = new List<DownloadViewer>();
                foreach(var d in downloads)
                {
                    views.Add(_itemsToDownloaders[d]);
                }
                return views;
            }
        }

        public List<DownloadViewer> AllDownloads
        {
            get
            {
                List<DownloadViewer> views = new List<DownloadViewer>();
                views = _itemsToDownloaders.Values.ToList();
                return views;
            }
        }

        public List<DownloadViewer> VideoDownloads
        {
            get
            {
                List<Downloader> downloads = _itemsToDownloaders.Keys.Where(d => d.FileType == Core.Concrete.Enums.FileType.Video).ToList();
                List<DownloadViewer> views = new List<DownloadViewer>();
                foreach (var d in downloads)
                {
                    views.Add(_itemsToDownloaders[d]);
                }
                return views;
                NotifyView();
            }
        }

        public List<DownloadViewer> DocumentDownloads
        {
            get
            {
                List<Downloader> downloads = _itemsToDownloaders.Keys.Where(d => d.FileType == Core.Concrete.Enums.FileType.Document).ToList();
                List<DownloadViewer> views = new List<DownloadViewer>();
                foreach (var d in downloads)
                {
                    views.Add(_itemsToDownloaders[d]);
                }
                return views;
            }
        }

        public List<DownloadViewer> ApplicationDownloads
        {
            get
            {
                List<Downloader> downloads = _itemsToDownloaders.Keys.Where(d => d.FileType == Core.Concrete.Enums.FileType.Application).ToList();
                List<DownloadViewer> views = new List<DownloadViewer>();
                foreach (var d in downloads)
                {
                    views.Add(_itemsToDownloaders[d]);
                }
                return views;
            }
        }

        public List<DownloadViewer> PictureDownloads
        {
            get
            {
                List<Downloader> downloads = _itemsToDownloaders.Keys.Where(d => d.FileType == Core.Concrete.Enums.FileType.Picture).ToList();
                List<DownloadViewer> views = new List<DownloadViewer>();
                foreach (var d in downloads)
                {
                    views.Add(_itemsToDownloaders[d]);
                }
                return views;
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

                NotifyView();

            }
            catch (FileNotFoundException)
            {
                //@
            }
        }
<<<<<<< HEAD
=======

        private void NotifyView()
        {
            NotifyPropertyChanged("ItemsToDownloaders");
            NotifyPropertyChanged("PictureDownloads");
            NotifyPropertyChanged("MusicDownloads");
            NotifyPropertyChanged("VideoDownloads");
            NotifyPropertyChanged("DocumentDownloads");
            NotifyPropertyChanged("ApplicationDownloads");
            NotifyPropertyChanged("AllDownloads");
        }
            

        public event PropertyChangedEventHandler PropertyChanged;
>>>>>>> origin/master

        public Dictionary<Downloader,DownloadViewer> ItemsToDownloaders
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
            NotifyView();
        }

        private static string GetFileName(ResourceInfo mirror)
        {
            Uri uri = new Uri(mirror.Url);
            var fileName = uri.Segments[uri.Segments.Length - 1];
            return HttpUtility.UrlDecode(fileName).Replace("/", "\\");
        }

        #endregion
    }
}
