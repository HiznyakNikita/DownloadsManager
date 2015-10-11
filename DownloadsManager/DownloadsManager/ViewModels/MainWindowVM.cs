using DownloadsManager.Core.Concrete;
using DownloadsManager.UserControls;
using DownloadsManager.ViewModels.Infrastructure;
using DownloadsManager.Views;
using System;
using System.Collections.Generic;
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
    public class MainWindowVM
    {
        private int currentDownloadIndex;

        /// <summary>
        /// ctor
        /// </summary>
        public MainWindowVM()
        {
            this.AddDownloadCmd = new Command(this.AddDownload);
        }

        /// <summary>
        /// Current download index
        /// </summary>
        public int CurrentDownloadIndex
        {
            get
            {
                return currentDownloadIndex;
            }
        }
            
        #region Commands

        /// <summary>
        /// Gets or sets Command for adding download
        /// </summary>
        public Command AddDownloadCmd { get; set; }

        /// <summary>
        /// Method for adding download
        /// </summary>
        /// <param name="param">Download param</param>
        private void AddDownload(object param)
        {
            currentDownloadIndex++;
            NewDownloadView newDownloadView = new NewDownloadView();
            newDownloadView.ShowDialog();
            ResourceInfo ri = new ResourceInfo();
            ri.URL = newDownloadView.UrlToDownload.ToString();
            Uri uri = new Uri(ri.URL);

            string fileName = uri.Segments[uri.Segments.Length - 1];
            fileName = HttpUtility.UrlDecode(fileName).Replace("/", "\\");
            DownloaderManager.Instance.Add(ri, null, @"D:\test", 100, true,fileName);

            //DownloadViewer viewer = new DownloadViewer();
            //(param as StackPanel).Children.Add(viewer);
            //viewer.DataContext = new DownloadViewerVM(DownloaderManager.Instance.Downloads[currentDownloadIndex]);
        }

        #endregion

    #region Methods
    #endregion
    }
}
