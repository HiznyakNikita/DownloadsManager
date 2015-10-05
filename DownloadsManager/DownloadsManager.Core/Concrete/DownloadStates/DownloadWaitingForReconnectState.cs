﻿using DownloadsManager.Core.Abstract;
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
    public class DownloadWaitingForReconnectState : IDownloaderState
    {
        private Downloader downloader;

        public DownloadWaitingForReconnectState(Downloader downloader)
        {
            this.downloader = downloader;
        }

        public void Pause()
        {
            downloader.FileSegments.Clear();
            downloader.MainThread.Abort();
            downloader.MainThread = null;
            downloader.SetState(new DownloadNeedToPrepareState(downloader));
            return;
        }

        public void Start()
        {
            throw new InvalidOperationException();
        }

        public void StartDownloadThread(object objSegmentCount)
        {
            downloader.SetState(new DownloadPreparingState(downloader));

            int segmentCount = Math.Min((int)objSegmentCount, Settings.Default.MaxSegmentCount);
            Stream inputStream = null;
            int currentTry = 0;

            do
            {
                downloader.LastError = null;

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
                    downloader.LastError = ex;
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
                downloader.LastError = null;
                downloader.StartSegments(segmentCount, inputStream);
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception ex)
            {
                downloader.LastError = ex;
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
            return "Waiting for reconnect";
        }
    }
}