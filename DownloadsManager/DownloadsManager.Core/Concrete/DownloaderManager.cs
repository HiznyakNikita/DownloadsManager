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
        /// <summary>
        /// dowloads
        /// </summary>
        private List<Downloader> downloads = new List<Downloader>();

        #region Properties

        /// <summary>
        /// Gets dowloads
        /// </summary>
        public IReadOnlyCollection<Downloader> Downloads 
        { 
            get
            {
                return downloads.AsReadOnly();
            }
        }
        #endregion

        /// <summary>
        /// add meyhod
        /// </summary>
        /// <param name="localFile">local file</param>
        /// <param name="remoteInfo">remote info</param>
        /// <param name="createdDateTime">reated dateime</param>
        /// <param name="uri">uri</param>
        /// <param name="path">path</param>
        /// <returns>Dowload</returns>
        public Downloader Add(string localFile, RemoteFileInfo remoteInfo, DateTime createdDateTime, string uri, string path)
        {
            Downloader d = new Downloader(localFile, remoteInfo, createdDateTime);
            downloads.Add(d);
            ////TODO Status changes
            d.Download(uri, path);
            return d;
        }
    }
}
