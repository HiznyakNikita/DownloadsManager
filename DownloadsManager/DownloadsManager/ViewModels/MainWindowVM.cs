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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
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
        private DateTime _historyPeriodFrom = DateTime.Now;
        private DateTime _historyPeriodTo = DateTime.Now;

        /// <summary>
        /// ctor
        /// </summary>
        public MainWindowVM()
        {
            this.AddDownloadCmd = new Command(this.AddDownload);
            this.CloseCmd = new Command(this.Close);
            this.ShowInFolderCmd = new Command(this.ShowInFolder);
            this.PauseAllDownloadCommand = new Command(PauseAllDownloads);
            this.RemoveAllCommand = new Command(RemoveAllDownloads);
            DownloaderManager.Instance.DownloadRemoved += DownloaderManager_DownloadRemoved;
            AddSavedDownloads();

        }

        void DownloaderManager_DownloadRemoved(object sender, EventArgs e)
        {
            List<Downloader> keys = _itemsToDownloaders.Keys.OfType<Downloader>().ToList();
            foreach(Downloader d in keys)
            {
                if (!DownloaderManager.Instance.Downloads.Contains(d))
                    _itemsToDownloaders.Remove(d);
                NotifyPropertyChanged("ItemsToDownloaders");
            }
        }

        public Command PauseAllDownloadCommand { get; set; }

        public Command RemoveAllCommand { get; set; }
        /// <summary>
        /// Gets or sets Command for adding download
        /// </summary>
        public Command AddDownloadCmd { get; set; }

        /// <summary>
        /// Command for closing app window and saving results
        /// </summary>
        public Command CloseCmd { get; set; }

        /// <summary>
        /// Command for showing download in folder
        /// </summary>
        public Command ShowInFolderCmd { get; set; }

        [field: NonSerialized]
        public event EventHandler DownloadEndedViewModel;


        public List<Downloader> DownloadsHistory
        { 
            get
            {
                return _itemsToDownloaders.Keys.OfType<Downloader>()
                    .Where(d => d.CreatedDateTime.CompareTo(_historyPeriodFrom) > 0 &&
                        d.CreatedDateTime.CompareTo(_historyPeriodTo) < 0).ToList();
            }
        }

        public Dictionary<DateTime,double> RatesStatistic
        {
            get 
            {
                Dictionary<DateTime,double> result = new Dictionary<DateTime,double>();
                foreach(var view in _itemsToDownloaders.Values)
                {
                    Dictionary<DateTime, double> viewRes = (view as DownloadViewer).Model.RatesStatistic;
                    foreach(KeyValuePair<DateTime,double> t in viewRes)
                    {
                        result.Add(t.Key, t.Value);
                    }
                }
                return result;
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

        public string TotalBytesDownloadedStatistic
        { 
            get
            {
                double result = _itemsToDownloaders.Keys.OfType<Downloader>().Select(d => d.FileSize).Sum();
                return result.ToString(new NumberFormatInfo()) + " bytes / " 
                    + Converters.BytesToKBConverter(result).ToString(new NumberFormatInfo()) + " KB / "
                    + Converters.BytesToMBConverter(result).ToString(new NumberFormatInfo()) + " MB / "
                    + Converters.BytesToGBConverter(result).ToString(new NumberFormatInfo()) + " GB";
            }
        }

        public Hashtable ItemsToDownloaders
        {
            get
            {
                return _itemsToDownloaders;
            }
        }

        public void AddHistoryParams(DateTime from, DateTime to)
        {
            _historyPeriodFrom = from;
            _historyPeriodTo = to;
            NotifyPropertyChanged("DownloadsHistory");
        }

        public Hashtable GetDownloaders()
        {
            return ItemsToDownloaders;
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

        public void OnDownloadEnded(object sender, EventArgs e)
        {
            if (DownloadEndedViewModel != null)
            {
                DownloadEndedViewModel(this, e);
            }
        }

        /// <summary>
        /// Method for adding download
        /// </summary>
        /// <param name="param">Download param</param>
        private void AddDownload(object param)
        {
            try
            {
                IWindowOpener windowOpener = new WindowOpener();
                windowOpener.OpenNewWindow(new NewDownloadVM());
                Downloader fileToDownload = DownloaderManager.Instance.LastDownload;
                fileToDownload.DownloadEnded += OnDownloadEnded;

                IControlCreator controlCreator = new ControlCreator();
                DownloadViewerVM model = new DownloadViewerVM(fileToDownload);
                var viewer = controlCreator.CreateControl(model);

                if (!_itemsToDownloaders.Keys.OfType<Downloader>().Contains(fileToDownload))
                {
                    _itemsToDownloaders.Add(fileToDownload, viewer);
                    NotifyPropertyChanged("ItemsToDownloaders");
                }

                NotifyPropertyChanged("TotalBytesDownloadedStatistic");
                NotifyPropertyChanged("DownloadStatesStatistic");
                NotifyPropertyChanged("DownloadTypesStatistic");
            }
            catch (ArgumentNullException)
            {

            }
        }

        private void RemoveAllDownloads(object param)
        {
            List<Downloader> keys = _itemsToDownloaders.Keys.OfType<Downloader>().ToList();
            foreach (var d in keys)
                _itemsToDownloaders.Remove(d);
            NotifyPropertyChanged("ItemsToDownloaders");
        }

        private void PauseAllDownloads(object param)
        {
            try
            {
                foreach (var d in _itemsToDownloaders.Keys)
                {
                    (d as Downloader).Pause();
                }
            }
            catch(InvalidOperationException)
            {

            }
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
