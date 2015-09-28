using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Core.Concrete
{
    /// <summary>
    /// Class for downloader management logic
    /// Uses for add downloads, Remove, Clear etc.
    /// </summary>
    public class DownloaderManager
    {
        private List<Downloader> downloads = new List<Downloader>();

        #region Properties
        public  IReadOnlyCollection<Downloader> Downloads 
        { 
            get
            {
                return downloads.AsReadOnly();
            }
        }
        #endregion

        public Downloader Add(string localFile, RemoteFileInfo remoteInfo, DateTime createdDateTime, string uri, string path)
        {
            Downloader d = new Downloader(localFile, remoteInfo, createdDateTime);
            downloads.Add(d);
            //TODO Status changes
            d.Download(uri, path);
            return d;
        }
    }
}
