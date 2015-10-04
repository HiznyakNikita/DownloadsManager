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
    }
}
