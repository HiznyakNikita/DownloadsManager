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
    public class DownloadEndedState : IDownloaderState
    {
        private Downloader downloader;

        public DownloadEndedState(Downloader downloader)
        {
            this.downloader = downloader;
        }

        /// <summary>
        /// Pause method implementation
        /// </summary>
        public void Pause() 
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Start method implementation
        /// </summary>
        public void Start() 
        {
            downloader.SetState(new DownloadPreparingState(downloader));

            downloader.StartPrepared();
        }

        /// <summary>
        /// StartDownloadThreadProc
        /// </summary>
        /// <param name="objSegmentCount">Segment count</param>
        public void StartDownloadThread(object startDownloadThreadParameter) 
        {
            downloader.SetState(new DownloadPreparingState(downloader));

            int segmentCount = Math.Min((int)startDownloadThreadParameter, Settings.Default.MaxSegmentCount);
            Stream inputStream = null;
            int currentTry = 0;

            do
            {
                downloader.DownloadingErrors= null;

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
            return "Ended";
        }
    }
}
