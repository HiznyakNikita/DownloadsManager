using DownloadsManager.Core.Abstract;
using DownloadsManager.Core.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DownloadsManager.Core.Concrete.DownloadStates
{
    [Serializable]
    public class DownloadDownloadingState : IDownloaderState
    {

         private Downloader downloader;

        public DownloadDownloadingState(Downloader downloader)
        {
            this.downloader = downloader;
        }

        public DownloadState State
        {
            get
            {
                return DownloadState.Working;
            }
        }

        public void Pause()
        {
            downloader.SetState(new DownloadPausingState(downloader));

            while (!downloader.AllWorkersStopped(5));

            lock (downloader.Threads)
            {
                downloader.Threads.Clear();
            }

            downloader.MainThread.Abort();
            downloader.MainThread = null;

            if (downloader.RemoteFileInfo != null && !downloader.RemoteFileInfo.AcceptRanges)
            {
                // reset the segment
                downloader.FileSegments[0].StartPosition = 0;
            }

            downloader.SetState(new DownloadPausedState(downloader));
        }

        public void Start()
        {
            throw new InvalidOperationException();
        }

        public void StartDownloadThread(object startDownloadThreadParameter)
        {
            downloader.SetState(new DownloadPreparingState(downloader));

            int segmentCount = Math.Min((int)startDownloadThreadParameter, Settings.Default.MaxSegmentCount);
            Stream inputStream = null;
            int currentTry = 0;

            do
            {
                downloader.DownloadingErrors = null;

                downloader.SetState(new DownloadPreparingState(downloader));

                currentTry++;
                try
                {
                    downloader.RemoteFileInfo = downloader.ProtocolProvider.GetFileInfo(downloader.ResourceInfo, out inputStream);
                    break;
                }
                catch (ThreadAbortException)
                {
                    downloader.SetState(new DownloadNeedToPrepareState(downloader));
                    return;
                }
                catch (Exception ex)
                {
                    downloader.DownloadingErrors = new AggregateException(ex);
                    if (currentTry < Settings.Default.MaxTries)
                    {
                        downloader.SetState(new DownloadWaitingForReconnectState(downloader));
                        Thread.Sleep(TimeSpan.FromSeconds(Settings.Default.RetrySleepInterval));
                    }
                    else
                    {
                        downloader.SetState(new DownloadNeedToPrepareState(downloader));
                        return;
                    }
                }
            }
            while (true);

            try
            {
                downloader.DownloadingErrors = null;
                downloader.StartSegments(segmentCount, inputStream);
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception ex)
            {
                downloader.DownloadingErrors = new AggregateException(ex);
                downloader.SetState(new DownloadEndedWithErrorState(downloader));
            }
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
            return "Downloading";
        }
    }
}
