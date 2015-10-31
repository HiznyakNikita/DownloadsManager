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
using System.Diagnostics;
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
    public class MainWindowVM : MainVM, IMainWindowVM
    {
        private Hashtable _itemsToDownloaders = new Hashtable();
        private int _historyPeriod = 1;

        /// <summary>
        /// ctor
        /// </summary>
        public MainWindowVM()
        {
            this.AddDownloadCmd = new Command(this.AddDownload);
            this.CloseCmd = new Command(this.Close);
            this.SetHistoryPeriodCmd = new Command(this.SetHistoryPeriod);
            this.ShowInFolderCmd = new Command(this.ShowInFolder);
            AddSavedDownloads();

        }

        /// <summary>
        /// Gets or sets Command for adding download
        /// </summary>
        public Command AddDownloadCmd { get; set; }

        /// <summary>
        /// Command for closing app window and saving results
        /// </summary>
        public Command CloseCmd { get; set; }

        /// <summary>
        /// Command for set value of days in downloads history period
        /// </summary>
        public Command SetHistoryPeriodCmd { get; set; }

        /// <summary>
        /// Command for showing download in folder
        /// </summary>
        public Command ShowInFolderCmd { get; set; }

        public List<Downloader> DownloadsHistory
        { 
            get
            {
                return _itemsToDownloaders.Keys.OfType<Downloader>()
                    .Where(d => d.CreatedDateTime.CompareTo(DateTime.Now.AddDays(-_historyPeriod)) > 0).ToList();
            }
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

        public Hashtable ItemsToDownloaders
        {
            get
            {
                return _itemsToDownloaders;
            }
        }

        private void AddSavedDownloads()
        {
            try
            {
                var downloads = DownloadsSerializer.Deserialize();
                foreach (var fileToDownload in downloads)
                {
                    if (fileToDownload.State.State != DownloadState.Ended
                        && fileToDownload.State.State != DownloadState.EndedWithError)
                    {
                        DownloaderManager.Instance.Add(fileToDownload, true);
                    }

                    using (var container = AutofacHelper.Container)
                    {
                        var controlCreator = new ControlCreator();
                        var viewer = controlCreator.CreateControl(new DownloadViewerVM(fileToDownload));
                        _itemsToDownloaders.Add(fileToDownload, viewer);
                    }
                }

                NotifyPropertyChanged("ItemsToDownloaders");
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException();
            }
        }

        #region Commands

        private void ShowInFolder(object param)
        {
            if (param != null)
            {
                Downloader download = param as Downloader;
                Process.Start(download.LocalFile);
            }
        }

        private void SetHistoryPeriod(object param)
        {
            if (!int.TryParse(param.ToString(), out _historyPeriod))
                throw new InvalidOperationException();
            NotifyPropertyChanged("DownloadsHistory");
        }

        private void Close(object param)
        {
            List<Downloader> downloadsToSave = new List<Downloader>();
            foreach (var download in _itemsToDownloaders.Keys.OfType<Downloader>().ToList())
            {
                try
                {
                    download.State.Pause();
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine();
                }

                downloadsToSave.Add(download);
            }

            DownloadsSerializer.Serialize(downloadsToSave);
        }

        public Hashtable GetDownloaders()
        {
            return ItemsToDownloaders;
        }

        /// <summary>
        /// Method for adding download
        /// </summary>
        /// <param name="param">Download param</param>
        private void AddDownload(object param)
        {
            IWindowOpener windowOpener = new WindowOpener();
            windowOpener.OpenNewWindow(new NewDownloadVM());
            Downloader fileToDownload = DownloaderManager.Instance.LastDownload;

            IControlCreator controlCreator = new ControlCreator();
            DownloadViewerVM model = new DownloadViewerVM(fileToDownload);
            var viewer = controlCreator.CreateControl(model);
            
            _itemsToDownloaders.Add(fileToDownload, viewer);
            NotifyPropertyChanged("ItemsToDownloaders");
        }

        #endregion

        #region Statistic
        private List<DownloadStatisticWrapper> GetTypesStatistic()
        {
            List<DownloadStatisticWrapper> result = new List<DownloadStatisticWrapper>();
            foreach (Downloader file in _itemsToDownloaders.Keys)
            {
                if (result.Where(d => d.FileType == file.FileType).Count() > 0)
                    result.Where(d => d.FileType == file.FileType).FirstOrDefault().Count++;
                else
                    result.Add(new DownloadStatisticWrapper(file.FileType, 1, file.State.State));
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
