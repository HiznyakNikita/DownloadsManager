using DownloadsManager.Core.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.ViewModels
{
    /// <summary>
    /// Main Window View Model
    /// </summary>
    public class MainWindowVM
    {
        private DownloaderManager downloadManager;

        private List<Downloader> downloads;

        public IReadOnlyCollection<Downloader> Downloads
        {
            get 
            {
                return downloads.AsReadOnly();
            }
        }

        public MainWindowVM()
        {
            downloadManager = new DownloaderManager();
            //FOR TEST DELETE AFTER TESTING
            downloadManager.Add("TEST FILE1", new RemoteFileInfo() { FileSize = 12, LastModified = DateTime.Now }, DateTime.Now, "http://www.codeproject.com/KB/architecture/362986/IteratorTestApp.zip", System.Reflection.Assembly.GetEntryAssembly().Location);
            downloadManager.Add("TEST FILE2", new RemoteFileInfo() { FileSize = 12, LastModified = DateTime.Now }, DateTime.Now, "http://www.codeproject.com/KB/architecture/362986/IteratorTestApp.zip", System.Reflection.Assembly.GetEntryAssembly().Location);
            downloadManager.Add("TEST FILE3", new RemoteFileInfo() { FileSize = 12, LastModified = DateTime.Now }, DateTime.Now, "http://www.codeproject.com/KB/architecture/362986/IteratorTestApp.zip", System.Reflection.Assembly.GetEntryAssembly().Location);

            downloads = downloadManager.Downloads.ToList();
        }
    }
}
