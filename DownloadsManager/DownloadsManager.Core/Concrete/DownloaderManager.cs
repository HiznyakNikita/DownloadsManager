﻿using DownloadsManager.Core.Concrete.DownloadStates;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private static DownloaderManager instance = new DownloaderManager();
        private static object lockObj = new object();
        private readonly List<Downloader> downloads = new List<Downloader>();

        private DownloaderManager() { }

        public static DownloaderManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {

                        if (instance == null)
                        {
                            instance = new DownloaderManager();
                        }
                    }
                }

                return instance;
            }
        }

        #region Properties

        public ReadOnlyCollection<Downloader> Downloads
        {
            get
            {
                return downloads.AsReadOnly();
            }
        }

        public double TotalDownloadRate
        {
            get
            {
                double total = 0;

                lock (lockObj)
                {
                    for (int i = 0; i < this.Downloads.Count; i++)
                    {
                        if (this.Downloads[i].State.GetType() == typeof(DownloadDownloadingState))
                        {
                            total += this.Downloads[i].Rate;
                        }
                    }
                }

                return total;
            }
        }

        #endregion

        #region Methods

        public void RemoveDownload(int index)
        {
            RemoveDownload(downloads[index]);
        }

        public void RemoveDownload(Downloader downloader)
        {
            if (downloader != null)
            {
                if (downloader.State.GetType() != typeof(DownloadNeedToPrepareState) ||
                    downloader.State.GetType() != typeof(DownloadEndedState) ||
                    downloader.State.GetType() != typeof(DownloadPausedState))
                {
                    downloader.Pause();
                }

                lock (lockObj)
                {
                    downloads.Remove(downloader);
                }
            }
        }

        public void ClearEnded()
        {
            lock (lockObj)
            {
                for (int i = downloads.Count - 1; i >= 0; i--)
                {
                    if (downloads[i].State.GetType() == typeof(DownloadEndedState))
                    {
                        downloads.RemoveAt(i);
                    }
                }
            }
        }

        public void PauseAll()
        {
            lock (lockObj)
            {
                for (int i = 0; i < this.Downloads.Count; i++)
                {
                    this.Downloads[i].Pause();
                }
            }
        }

        public Downloader Add(ResourceInfo resourceInfo, ResourceInfo[] mirrors, string localFile, bool autoStart, string fileName)
        {
            Downloader d = new Downloader(resourceInfo, mirrors, localFile, fileName);
            Add(d, autoStart);

            return d;
        }

        public Downloader Add(
            ResourceInfo resourceInfo, 
            ResourceInfo[] mirrors, 
            string localFile, 
            List<FileSegment> segments, 
            RemoteFileInfo remoteInfo, 
            int requestedSegmentCount, 
            bool autoStart, 
            DateTime createdDateTime,
            string fileName)
        {
            Downloader d = new Downloader(resourceInfo, mirrors, localFile, segments, remoteInfo, createdDateTime, fileName);
            Add(d, autoStart);

            return d;
        }

        public void Add(Downloader downloader, bool autoStart)
        {
            if (downloader != null)
            {
                lock (lockObj)
                {
                    downloads.Add(downloader);
                }

                if (autoStart)
                {
                    downloader.Start();
                }
            }
        }

        public void SwapDownloads(int index)
        {
            lock (lockObj)
            {
                InternalSwap(index);
            }
        }

        private void InternalSwap(int idx)
        {
            if (this.downloads.Count <= idx)
            {
                throw new IndexOutOfRangeException();
            }

            Downloader it1 = this.downloads[idx];
            Downloader it2 = this.downloads[idx - 1];

            this.downloads.RemoveAt(idx);
            this.downloads.RemoveAt(idx - 1);

            this.downloads.Insert(idx - 1, it1);
            this.downloads.Insert(idx, it2);
        }

        #endregion
    }
}
