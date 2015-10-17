using DownloadsManager.Core.Abstract;
using DownloadsManager.Core.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Core.Concrete.DownloadStates
{
    [Serializable]
    public class DownloadPausingState : IDownloaderState
    {
        private Downloader downloader;

        public DownloadPausingState(Downloader downloader)
        {
            this.downloader = downloader;
        }

        public void Pause()
        {
            throw new InvalidOperationException();
        }

        public void Start()
        {
            throw new InvalidOperationException();
        }

        public void StartDownloadThread(object startDownloadThreadParameter)
        {
            downloader.SetState(new DownloadPreparingState(downloader));

            downloader.DownloadingErrors= null;
            downloader.SetState(new DownloadNeedToPrepareState(downloader));
            return;
        }

        /// <summary>
        /// Set state of downloader
        /// </summary>
        /// <param name="state">state to change</param>
        public void SetState(IDownloaderState state)
        {
            downloader.SetState(state);
        }

        public override string ToString()
        {
            return "Pausing";
        }
    }
}
