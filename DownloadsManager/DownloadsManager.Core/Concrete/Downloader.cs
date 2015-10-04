using DownloadsManager.Core.Abstract;
using DownloadsManager.Core.Concrete.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DownloadsManager.Core.Concrete
{
    /// <summary>
    /// Class which represent every download
    /// TODO State,Download providers, segments (for pause and speed manipulations)
    /// </summary>
    public class Downloader : IDownloader
    {
        private string localFile;
        private int requestedFileSegmentCount;
        private ResourceInfo resourceInfo;
        private Thread mainThread;
        private RemoteFileInfo remoteFileInfo;
        private IDownloaderState state;
        private DateTime createdDateTime;
        private Exception lastDownloadingError;

        private List<ResourceInfo> mirrors;
        private List<FileSegment> segments;
        private List<Thread> threads;

        private IProtocolProvider currentDownloadProvider;
        private IFileSegmentCalculator fileSegmentCalculator;

        //private string statusMessage;

        /// <summary>
        /// default ctor (super)
        /// </summary>
        /// <param name="ri">resource info</param>
        /// <param name="mirrors">resource info mirrors</param>
        /// <param name="localFile">local file for this downloader</param>
        private Downloader( ResourceInfo ri, ResourceInfo[] mirrors, string localFile)
        {
            this.threads = new List<Thread>();
            this.resourceInfo = ri;
            if (mirrors == null)
            {
                this.mirrors = new List<ResourceInfo>();
            }
            else
            {
                this.mirrors = new List<ResourceInfo>(mirrors);
            }
            this.localFile = localFile;

            currentDownloadProvider = ri.BindProtocolProviderInstance();

            fileSegmentCalculator = new FileSegmentSizeCalculatorHelper();
        }

        /// <summary>
        /// ctor for segment downloading need for change downloader statuses
        /// </summary>
        /// <param name="ri">resource info</param>
        /// <param name="mirrors">mirrors for downloading</param>
        /// <param name="localFile">local file for downloader</param>
        /// <param name="segmentCount">count of segments</param>
        public Downloader( ResourceInfo ri, ResourceInfo[] mirrors, string localFile, int segmentCount) : this(ri, mirrors, localFile)
        {
            //SetState(DownloaderState.NeedToPrepare);

            this.createdDateTime = DateTime.Now;
            this.requestedFileSegmentCount = segmentCount;
            this.segments = new List<FileSegment>();
        }

        /// <summary>
        /// more complicated ctor need for change downloader statuses
        /// </summary>
        /// <param name="ri">resource info</param>
        /// <param name="mirrors">mirrors for downloading</param>
        /// <param name="localFile">local file info for downloading</param>
        /// <param name="segments">segments for downloading</param>
        /// <param name="remoteInfo">remote file info (place here after getting file info from server)</param>
        /// <param name="requestedSegmentCount">segment ount for request</param>
        /// <param name="createdDateTime">date time of creating dowloader</param>
        public Downloader( ResourceInfo ri, ResourceInfo[] mirrors, string localFile, List<FileSegment> segments, RemoteFileInfo remoteInfo, 
            int requestedSegmentCount, DateTime createdDateTime) : this(ri, mirrors, localFile)
        {
            if (segments.Count > 0)
            {
                //SetState(DownloaderState.Prepared);
            }
            else
            {
                //SetState(DownloaderState.NeedToPrepare);
            }

            this.createdDateTime = createdDateTime;
            this.remoteFileInfo = remoteInfo;
            this.requestedFileSegmentCount = requestedSegmentCount;
            this.segments = segments;
        }

        #region Properties

        /// <summary>
        /// Gets ResourceInfo for downloader
        /// </summary>
        public ResourceInfo ResourceInfo
        {
            get
            {
                return this.resourceInfo;
            }
        }

        /// <summary>
        /// Gets mirrors of downloader
        /// </summary>
        public List<ResourceInfo> Mirrors
        {
            get
            {
                return this.mirrors;
            }
        }

        /// <summary>
        /// Gets downloading file size
        /// </summary>
        public long FileSize
        {
            get
            {
                if (remoteFileInfo == null)
                {
                    return 0;
                }
                return remoteFileInfo.FileSize;
            }
        }

        /// <summary>
        /// Gets date time of downloader creating
        /// </summary>
        public DateTime CreatedDateTime
        {
            get
            {
                return createdDateTime;
            }
        }

        /// <summary>
        /// Gets segments of request
        /// </summary>
        public int RequestedSegments
        {
            get
            {
                return requestedFileSegmentCount;
            }
        }

        /// <summary>
        /// Gets local file info
        /// </summary>
        public string LocalFile
        {
            get
            {
                return this.localFile;
            }
        }

        /// <summary>
        /// Gets progress of downloading
        /// </summary>
        public double Progress
        {
            get
            {
                int count = segments.Count;

                if (count > 0)
                {
                    double progress = 0;

                    for (int i = 0; i < count; i++)
                    {
                        ////add progress of all segments
                        progress += segments[i].Progress;
                    }

                    ////percent of progress
                    return progress / count;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Gets rate of downloadng
        /// </summary>
        public double Rate
        {
            get
            {
                double rate = 0;

                for (int i = 0; i < segments.Count; i++)
                {
                    ////sum rates of all file segments
                    rate += segments[i].Rate;
                }

                return rate;
            }
        }

        /// <summary>
        /// Gets calculating value of transfered bytes
        /// </summary>
        public long Transfered
        {
            get
            {
                long transfered = 0;

                for (int i = 0; i < segments.Count; i++)
                {
                    ////sum transfered bytes of all segments
                    transfered += segments[i].Transfered;
                }

                return transfered;
            }
        }

        /// <summary>
        /// Gets calculated value of left downloading time
        /// </summary>
        public TimeSpan Left
        {
            get
            {
                if (this.Rate == 0)
                {
                    return TimeSpan.MaxValue;
                }

                double missingTransfer = 0;

                for (int i = 0; i < segments.Count; i++)
                {
                    missingTransfer += segments[i].MissingTransfer;
                }

                return TimeSpan.FromSeconds(missingTransfer / this.Rate);
            }
        }

        public List<FileSegment> FileSegments
        {
            get
            {
                return segments;
            }
        }

        public Exception LastError
        {
            get { return lastDownloadingError; }
            set { lastDownloadingError = value; }
        }

        public IDownloaderState State
        {
            get { return state; }
        }

        //TODO Uncomment this after implementing downloading states
        //public bool IsWorking()
        //{
        //    DownloaderState state = this.State;
        //    return (state == DownloaderState.Preparing ||
        //        state == DownloaderState.WaitingForReconnect ||
        //        state == DownloaderState.Working);
        //}

        /// <summary>
        /// Gets remote file info
        /// </summary>
        public RemoteFileInfo RemoteFileInfo
        {
            get { return remoteFileInfo; }
        }

        /// <summary>
        /// Gets or sets segments calculator for downloader
        /// </summary>
        public IFileSegmentCalculator SegmentCalculator
        {
            get { return fileSegmentCalculator; }
            set
            {
                fileSegmentCalculator = value;
            }
        }

        #endregion

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="uri">uri</param>
        /// <param name="path">path</param>
        public void Download(string uri, string path)
        {
            int currentTry = 0;

            do
            {
                currentTry++;
                try
                {
                    remoteFileInfo = currentDownloadProvider.GetFileInfo(this.resourceInfo);
                    break;
                }
                catch (ThreadAbortException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                }
            }
            while (true);
        }

        /// <summary>
        /// complete method
        /// </summary>
        /// <param name="sender">sender download</param>
        /// <param name="e">args</param>
        private void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Debug.WriteLine("Download completed");
        }
    }
}
