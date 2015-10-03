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
        /// <summary>
        /// List of pownloads
        /// </summary>
        private List<Downloader> downloads;

        /// <summary>
        /// ctor
        /// </summary>
        public MainWindowVM()
        {

        }

        /// <summary>
        /// Gets DownloadsCollection
        /// </summary>
        public IReadOnlyCollection<Downloader> Downloads
        {
            get
            {
                return downloads.AsReadOnly();
            }
        }
    }
}
