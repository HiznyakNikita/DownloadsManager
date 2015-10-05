using DownloadsManager.Core.Abstract;
using DownloadsManager.Core.Concrete.DownloadStates;
using DownloadsManager.Core.Concrete.Enums;
using DownloadsManager.Core.Concrete.Helpers;
using DownloadsManager.Core.Properties;
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
        
        /// <summary>
        ///  Need for getting next mirror in GetNextResource method 
        /// </summary>
        private int mirrorCounter;

        private List<ResourceInfo> mirrors;
        private List<FileSegment> segments;
        private List<Thread> threads;

        private IProtocolProvider currentDownloadProvider;
        private IFileSegmentCalculator fileSegmentCalculator;

        /// <summary>
        /// ctor for segment downloading need for change downloader statuses
        /// </summary>
        /// <param name="ri">resource info</param>
        /// <param name="mirrors">mirrors for downloading</param>
        /// <param name="localFile">local file for downloader</param>
        /// <param name="segmentCount">count of segments</param>
        public Downloader(ResourceInfo ri, ResourceInfo[] mirrors, string localFile, int segmentCount) : this(ri, mirrors, localFile)
        {
            ////SetState(DownloaderState.NeedToPrepare);

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
        public Downloader(
            ResourceInfo ri,
            ResourceInfo[] mirrors,
            string localFile,
            List<FileSegment> segments,
            RemoteFileInfo remoteInfo,
            int requestedSegmentCount,
            DateTime createdDateTime)
            : this(ri, mirrors, localFile)
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

        /// <summary>
        /// default ctor (super)
        /// </summary>
        /// <param name="ri">resource info</param>
        /// <param name="mirrors">resource info mirrors</param>
        /// <param name="localFile">local file for this downloader</param>
        private Downloader(ResourceInfo ri, ResourceInfo[] mirrors, string localFile)
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
        /// Gets work download threads
        /// </summary>
        public List<Thread> Threads
        {
            get 
            {
                return threads;
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
        /// Gets or sets remote file info
        /// </summary>
        public RemoteFileInfo RemoteFileInfo
        {
            get { return remoteFileInfo; }
            set { remoteFileInfo = value; }
        }

        /// <summary>
        /// Gets download provider for downloader
        /// </summary>
        public IProtocolProvider ProtocolProvider
        {
            get { return currentDownloadProvider; }
        }

        /// <summary>
        /// Gets or sets segments calculator for downloader
        /// </summary>
        public IFileSegmentCalculator SegmentCalculator
        {
            get { return fileSegmentCalculator; }
            set { fileSegmentCalculator = value; }
        }

        /// <summary>
        /// Gets or sets main thread
        /// </summary>
        public Thread MainThread
        {
            get
            {
                return mainThread;
            }

            set
            {
                mainThread = value;
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
            Stream downloadStream;
            int currentTry = 0;

            do
            {
                currentTry++;
                try
                {
                    remoteFileInfo = currentDownloadProvider.GetFileInfo(this.resourceInfo, out downloadStream);
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
        /// Sets state of downloader
        /// </summary>
        /// <param name="state">State of downloader</param>
        public void SetState(IDownloaderState state)
        {
            if (state != null)
            {
                this.state = state;
            }
        }

        /// <summary>
        /// start prepeare fow download
        /// startDownloadThread method of state starting
        /// </summary>
        public void StartToPrepare()
        {
            mainThread = new Thread(new ParameterizedThreadStart(state.StartDownloadThread));
            mainThread.Start(requestedFileSegmentCount);
        }

        /// <summary>
        /// start already prepeared downloader for file downloading
        /// </summary>
        public void StartPrepared()
        {
            mainThread = new Thread(new ThreadStart(RestartDownload));
            mainThread.Start();
        }

        public void StartSegments(int segmentCount, Stream inputStream)
        {
            ////Pre-downloading locate file on local disk
            LocateLocalFile();

            long segmentSize;

            List<CalculatedFileSegment> calculatedSegments;

            if (!remoteFileInfo.AcceptRanges)
            {
                calculatedSegments = new List<CalculatedFileSegment> { new CalculatedFileSegment(0, remoteFileInfo.FileSize) };
            }
            else
            {
                calculatedSegments = this.SegmentCalculator.GetSegments(segmentCount, remoteFileInfo);
            }

            lock (threads)
            {
                threads.Clear();
            }

            lock (segments)
            {
                segments.Clear();
            }

            for (int i = 0; i < calculatedSegments.Count; i++)
            {
                FileSegment segment = new FileSegment();
                if (i == 0)
                {
                    segment.InputStream = inputStream;
                }

                segment.Index = i;
                segment.InitialStartPosition = calculatedSegments[i].SegmentStartPosition;
                segment.StartPosition = calculatedSegments[i].SegmentStartPosition;
                segment.EndPosition = calculatedSegments[i].SegmentEndPosition;

                segments.Add(segment);
            }

            RunSegments();
        }

        public bool AllWorkersStopped(int timeOut)
        {
            bool allFinished = true;

            List<Thread> workThreads;

            lock (threads)
            {
                workThreads = threads;
            }

            foreach (Thread t in workThreads)
            {
                bool finished = t.Join(timeOut);
                allFinished = allFinished & finished;

                if (finished)
                {
                    lock (threads)
                    {
                        threads.Remove(t);
                    }
                }
            }

            return allFinished;
        }

        public void Start()
        {
            try
            {
                state.Start();
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public void Pause()
        {
            try
            {
                state.Pause();
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public void StartDownloadThread(object objSegmentCount)
        {
            try
            {
                state.StartDownloadThread(objSegmentCount);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        private void RunSegments()
        {
            state.SetState(new DownloadWorkingState(this));

            using (FileStream fs = new FileStream(this.LocalFile, FileMode.Open, FileAccess.Write))
            {
                for (int i = 0; i < this.FileSegments.Count; i++)
                {
                    FileSegments[i].OutputStream = fs;
                    StartSegment(FileSegments[i]);
                }

                do
                {
                    while (!AllWorkersStopped(1000));
                }
                while (RestartFailedSegments());
            }

            for (int i = 0; i < this.FileSegments.Count; i++)
            {
                if (FileSegments[i].State == FileSegmentState.Error)
                {
                    state.SetState(new DownloadEndedWithErrorState(this));
                    return;
                }
            }

            state.SetState(new DownloadEndedState(this));
        }

        private bool RestartFailedSegments()
        {
            bool hasErrors = false;
            double delay = 0;

            for (int i = 0; i < this.FileSegments.Count; i++)
            {
                if (FileSegments[i].State == FileSegmentState.Error &&
                    FileSegments[i].LastError != null &&
                    (Settings.Default.MaxTries == 0 ||
                    FileSegments[i].CurrentTry < Settings.Default.MaxTries))
                {
                    hasErrors = true;
                    TimeSpan ts = DateTime.Now - FileSegments[i].LastErrorTime;

                    if (ts.TotalSeconds >= Settings.Default.RetrySleepInterval)
                    {
                        FileSegments[i].CurrentTry++;
                        StartSegment(FileSegments[i]);
                    }
                    else
                    {
                        delay = Math.Max(delay, (Settings.Default.RetrySleepInterval * 1000) - ts.TotalMilliseconds);
                    }
                }
            }

            Thread.Sleep((int)delay);

            return hasErrors;
        }

        private void StartSegment(FileSegment newSegment)
        {
            Thread segmentThread = new Thread(new ParameterizedThreadStart(FileSegmentThreadProc));
            segmentThread.Start(newSegment);

            lock (threads)
            {
                threads.Add(segmentThread);
            }
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

        private void RestartDownload()
        {
            Stream downloadStream;
            int restartTriesCounter = 0;
            ////need for check if information on server have changed while waiting for restart
            RemoteFileInfo reloadedFileInfo;

            try
            {
                do
                {
                    state.SetState(new DownloadPreparingState(this));

                    restartTriesCounter++;
                    try
                    {
                        reloadedFileInfo = currentDownloadProvider.GetFileInfo(this.ResourceInfo, out downloadStream);
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (restartTriesCounter < Settings.Default.MaxTries)
                        {
                            state.SetState(new DownloadWaitingForReconnectState(this));
                            Thread.Sleep(TimeSpan.FromSeconds(Settings.Default.RetrySleepInterval));
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                while (true);
            }
            finally
            {
                state.SetState(new DownloadPreparedState(this));
            }

            try
            {
                // check if the file already have changed on the server
                if (!reloadedFileInfo.AcceptRanges ||
                    reloadedFileInfo.LastModified > RemoteFileInfo.LastModified ||
                    reloadedFileInfo.FileSize != RemoteFileInfo.FileSize)
                {
                    this.remoteFileInfo = reloadedFileInfo;
                    StartSegments(this.RequestedSegments, downloadStream);
                }
                else
                {
                    if (downloadStream != null)
                    {
                        downloadStream.Dispose();
                    }

                    RunSegments();
                }
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception ex)
            {
                state.SetState(new DownloadEndedWithErrorState(this));
            }
        }

        private void FileSegmentThreadProc(object objFileSegment)
        {
            FileSegment segment = (FileSegment)objFileSegment;
            segment.LastError = null;
            try
            {
                if (segment.EndPosition > 0 && segment.StartPosition >= segment.EndPosition)
                {
                    segment.State = FileSegmentState.Finished;

                    return;
                }

                int buffSize = 8192;
                byte[] buffer = new byte[buffSize];
                segment.State = FileSegmentState.Connecting;
                if (segment.InputStream == null)
                {
                    //// get the next URL (It can the the main url or some mirror)
                    ResourceInfo resourceInfo = GetNextResourceInfo();
                    //// get the protocol provider for that mirror
                    IProtocolProvider provider = resourceInfo.BindProtocolProviderInstance();

                    while (resourceInfo != this.ResourceInfo)
                    {
                        Stream tempStream;

                        //// get the remote file info on mirror
                        RemoteFileInfo tempRemoteInfo = provider.GetFileInfo(resourceInfo, out tempStream);
                        if (tempStream != null)
                        {
                            tempStream.Dispose();
                        }

                        //// check if the file on mirror is the same
                        if (tempRemoteInfo.FileSize == remoteFileInfo.FileSize &&
                            tempRemoteInfo.AcceptRanges == remoteFileInfo.AcceptRanges)
                        {
                            //// if yes, stop looking for the mirror
                            break;
                        }

                        lock (mirrors)
                        {
                            //// the file on the mirror is not the same, so remove from the mirror list
                            mirrors.Remove(resourceInfo);
                        }

                        //// the file on the mirror is different
                        //// so get other mirror to use in the segment
                        resourceInfo = GetNextResourceInfo();
                        provider = resourceInfo.BindProtocolProviderInstance();
                    }

                    //// get the input stream from start position
                    segment.InputStream = provider.CreateResponseStream(resourceInfo, (int)segment.StartPosition, (int)segment.EndPosition);

                    //// change the segment URL to the mirror URL
                    segment.CurrentURL = resourceInfo.URL;
                }
                else
                {
                    ////  change the segment URL to the main URL
                    segment.CurrentURL = this.resourceInfo.URL;
                }

                ReadSegment(segment, buffSize, buffer);

            }
            catch (Exception ex)
            {
                //// store the error information
                segment.State = FileSegmentState.Error;
                segment.LastError = ex;

                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                //// clean up the segment
                segment.InputStream = null;
            }
        }

        private void ReadSegment(FileSegment segment, int buffSize, byte[] buffer)
        {
            using (segment.InputStream)
            {
                //// change the segment state
                segment.State = FileSegmentState.Downloading;
                segment.CurrentTry = 0;
                long readSize;
                do
                {
                    //// reads the buffer from input stream
                    readSize = segment.InputStream.Read(buffer, 0, buffSize);
                    //// check if the segment has reached the end
                    if (segment.EndPosition > 0 &&
                        segment.StartPosition + readSize > segment.EndPosition)
                    {
                        //// adjust the 'readSize' to write only necessary bytes
                        readSize = segment.EndPosition - segment.StartPosition;
                        if (readSize <= 0)
                        {
                            segment.StartPosition = segment.EndPosition;
                            break;
                        }
                    }

                    //// locks the stream to avoid that other threads changes
                    //// the position of stream while this thread is writing into the stream
                    lock (segment.OutputStream)
                    {
                        segment.OutputStream.Position = segment.StartPosition;
                        segment.OutputStream.Write(buffer, 0, (int)readSize);
                    }

                    //// increse the start position of the segment and also calculates the rate
                    segment.IncreaseStartPosition(readSize);
                    //// check if the stream has reached its end
                    if (segment.EndPosition > 0 && segment.StartPosition >= segment.EndPosition)
                    {
                        segment.StartPosition = segment.EndPosition;
                        break;
                    }

                    //// check if the user have requested to pause the download
                    if (state.GetType() == typeof(DownloadPausingState))
                    {
                        segment.State = FileSegmentState.Paused;
                        break;
                    }

                }
                while (readSize > 0);
                if (segment.State == FileSegmentState.Downloading)
                {
                    segment.State = FileSegmentState.Finished;

                    //// try to create other segment, 
                    //// spliting the missing bytes from one existing segment
                    AddNewSegmentIfNeeded();
                }
            }
        }

        private void AddNewSegmentIfNeeded()
        {
            lock (segments)
            {
                for (int i = 0; i < this.segments.Count; i++)
                {
                    FileSegment oldSegment = this.segments[i];
                    if (oldSegment.State == FileSegmentState.Downloading &&
                        oldSegment.TimeLeft.TotalSeconds > Settings.Default.MinSegmentWidthToNewSegment &&
                        oldSegment.MissingTransfer / 2 >= Settings.Default.MinSegmentSize)
                    {
                        //// get the half of missing size of oldSegment
                        long newSize = oldSegment.MissingTransfer / 2;

                        //// create a new segment allocation the half old segment
                        FileSegment newSegment = new FileSegment();
                        newSegment.Index = this.segments.Count;
                        newSegment.StartPosition = oldSegment.StartPosition + newSize;
                        newSegment.InitialStartPosition = newSegment.StartPosition;
                        newSegment.EndPosition = oldSegment.EndPosition;
                        newSegment.OutputStream = oldSegment.OutputStream;

                        //// removes bytes from old segments
                        oldSegment.EndPosition = oldSegment.EndPosition - newSize;

                        //// add the new segment to the list
                        segments.Add(newSegment);

                        StartSegment(newSegment);

                        break;
                    }
                }
            }
        }

        #region HelperMethods

        private void LocateLocalFile()
        {
            if (!Directory.Exists(this.LocalFile))
            {
                Directory.CreateDirectory(this.LocalFile);
            }

            if (new FileInfo(this.LocalFile).Exists)
            {
                int currentVersion = 0;
                string newFileName = this.LocalFile;
                do
                {
                    newFileName = this.LocalFile + currentVersion.ToString();
                }
                while (new FileInfo(newFileName).Exists);

                this.localFile = newFileName;
            }

            using (FileStream fs = new FileStream(this.LocalFile, FileMode.Create, FileAccess.Write))
            {
                fs.SetLength(Math.Max(this.FileSize, 0));
            }
        }

        /// <summary>
        /// Need for get next mirror if files in mirror and main download are different
        /// </summary>
        /// <returns>Resource information</returns>
        private ResourceInfo GetNextResourceInfo()
        {
            if (Mirrors == null || Mirrors.Count == 0)
            {
                return this.ResourceInfo;
            }

            lock (Mirrors)
            {
                if (mirrorCounter >= Mirrors.Count)
                {
                    mirrorCounter = 0;

                    return this.ResourceInfo;
                }

                return Mirrors[mirrorCounter++];
            }
        }

        #endregion
    }
}
