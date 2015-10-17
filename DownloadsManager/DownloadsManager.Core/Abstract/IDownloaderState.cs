using DownloadsManager.Core.Concrete;
using DownloadsManager.Core.Concrete.DownloadStates;
using DownloadsManager.Core.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DownloadsManager.Core.Abstract
{
    public interface IDownloaderState
    {
        /// <summary>
        /// Change state of object
        /// </summary>
        /// <param name="state">state to change</param>
        void SetState(IDownloaderState state);

        /// <summary>
        /// Method for starting download
        /// </summary>
        void Start();
        
        /// <summary>
        /// Method for starting download thread
        /// </summary>
        /// <param name="startDownloadThreadParameter">param to start</param>
        void StartDownloadThread(object startDownloadThreadParameter);

        /// <summary>
        /// Method for paused download
        /// </summary>
        void Pause();
    }

}
