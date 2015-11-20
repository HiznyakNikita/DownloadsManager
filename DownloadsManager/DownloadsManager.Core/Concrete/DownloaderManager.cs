using DownloadsManager.Core.Concrete.DownloadStates;
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

        private DownloaderManager()
        {
        }

        [field: NonSerialized]
        public event EventHandler DownloadRemoved;

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

        public Downloader LastDownload
        {
            get;
            private set;
        }

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
                        if (this.Downloads[i].State.State == DownloadState.Working)
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
                if (downloader.State.State != DownloadState.NeedToPrepare ||
                    downloader.State.State != DownloadState.Ended ||
                    downloader.State.State != DownloadState.Paused)
                {
                    try
                    {
                        downloader.Pause();
                    }
                    catch (InvalidOperationException)
                    {
                        
                    }
                }

                lock (lockObj)
                {
                    downloads.Remove(downloader);
                }
            }

            OnDownloadRemoved();
        }

        public void ClearEnded()
        {
            lock (lockObj)
            {
                for (int i = downloads.Count - 1; i >= 0; i--)
                {
                    if (downloads[i].State.State == DownloadState.Ended)
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
                    LastDownload = downloader;
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

        protected virtual void OnDownloadRemoved()
        {
            if (DownloadRemoved != null)
            {
                DownloadRemoved(this, EventArgs.Empty);
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
