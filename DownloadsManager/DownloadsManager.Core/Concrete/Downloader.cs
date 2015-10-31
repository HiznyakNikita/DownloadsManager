using DownloadsManager.Core.Abstract;
using DownloadsManager.Core.Concrete.DownloadStates;
using DownloadsManager.Core.Concrete;
using DownloadsManager.Core.Concrete;
using DownloadsManager.Core.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using System.Xml.Serialization;
using DownloadsManager.Core.Concrete.Enums;
using DownloadsManager.Core.Concrete.Helpers;

namespace DownloadsManager.Core.Concrete
{
    [CLSCompliant(true)]
    [Serializable]

    /// <summary>
    /// Class which represent every download
    /// </summary>
    public class Downloader : IDownloader, INotifyPropertyChanged, IEquatable<Downloader>
    {
        [NonSerialized]
        private Thread mainThread;
        private RemoteFileInfo remoteFileInfo;
        [NonSerialized]
        private AggregateException downloadingErrors;
        private IDownloaderState state;

        private object syncObject = new object();
        
        /// <summary>
        ///  Need for getting next mirror in GetNextResource method 
        /// </summary>
        private int mirrorCounter;

        private List<ResourceInfo> mirrors;
        private List<FileSegment> segments;
        [NonSerialized]
        private List<Thread> threads = new List<Thread>();

        private IFileSegmentCalculator fileSegmentCalculator;

        public Downloader() { }

        /// <summary>
        /// more complicated ctor need for change downloader statuses
        /// </summary>
        /// <param name="resourceInfo">resource info</param>
        /// <param name="mirrors">mirrors for downloading</param>
        /// <param name="localFile">local file info for downloading</param>
        /// <param name="segments">segments for downloading</param>
        /// <param name="remoteInfo">remote file info (place here after getting file info from server)</param>
        /// <param name="createdDateTime">date time of creating dowloader</param>
        /// <param name="fileName">name of file</param>
        public Downloader(
            ResourceInfo resourceInfo,
            ResourceInfo[] mirrors,
            string localFile,
            List<FileSegment> segments,
            RemoteFileInfo remoteInfo,
            DateTime createdDateTime,
            string fileName)
            : this(resourceInfo, mirrors, localFile, fileName)
        {
            if (segments != null && segments.Count > 0)
            {
                State.SetState(new DownloadPreparedState(this));
            }
            else
            {
                State.SetState(new DownloadNeedToPrepareState(this));
            }

            CreatedDateTime = createdDateTime;
            this.remoteFileInfo = remoteInfo;
            RequestedSegments = Settings.Default.DefaultSegmentsCount;
            this.segments = segments;
            State = new DownloadNeedToPrepareState(this);
            FileName = fileName;

        }

        /// <summary>
        /// default ctor (super)
        /// </summary>
        /// <param name="resourceInfo">resource info</param>
        /// <param name="mirrors">resource info mirrors</param>
        /// <param name="localFile">local file for this downloader</param>
        /// <param name="fileName">name of file</param>
        public Downloader(ResourceInfo resourceInfo, ResourceInfo[] mirrors, string localFile, string fileName)
        {
            CreatedDateTime = DateTime.Now;
            RequestedSegments = Settings.Default.DefaultSegmentsCount;
            this.segments = new List<FileSegment>();
            State = new DownloadNeedToPrepareState(this);
            FileName = fileName;

            this.threads = new List<Thread>();
            ResourceInfo = resourceInfo;
            this.mirrors = mirrors == null 
                ? new List<ResourceInfo>()
                : new List<ResourceInfo>(mirrors);

            LocalFile = localFile;
            if (resourceInfo != null)
                ProtocolProvider = resourceInfo.BindProtocolProviderInstance();

            fileSegmentCalculator = new FileSegmentSizeCalculatorHelper();
            FileName = fileName;
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties

        /// <summary>
        /// Gets ResourceInfo for downloader
        /// </summary>
        public ResourceInfo ResourceInfo
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets name of download
        /// </summary>
        public string FileName
        {
            get;
            private set;
        }

        public FileType FileType 
        { 
            get
            {
                return FileTypeIdentifier.IdentifyType(FileName);
            }
        }

        /// <summary>
        /// Gets mirrors of downloader
        /// </summary>
        public List<ResourceInfo> Mirrors
        {
            get
            {
                return mirrors;
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
            get;
            private set;
        }

        /// <summary>
        /// Gets segments of request
        /// </summary>
        public int RequestedSegments
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets local file info
        /// </summary>
        public string LocalFile
        {
            get;
            private set;
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

        [XmlIgnore]

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
        public long TransferBytes
        {
            get
            {
                long transfered = 0;

                for (int i = 0; i < segments.Count; i++)
                {
                    ////sum transfered bytes of all segments
                    transfered += segments[i].TransferBytes;
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

        [XmlIgnore]
        public AggregateException DownloadingErrors
        {
            get { return downloadingErrors; }
            set { downloadingErrors = value; }
        }

        public IDownloaderState State
        {
            get
            {
                return state;
            }

            set
            {
                state = value;
            }
        }

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
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets segments calculator for downloader
        /// </summary>
        public IFileSegmentCalculator SegmentCalculator
        {
            get { return fileSegmentCalculator; }
            set { fileSegmentCalculator = value; }
        }

        [XmlIgnore]

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
                    remoteFileInfo = ProtocolProvider.GetFileInfo(ResourceInfo, out downloadStream);
                    break;
                }
                catch (ThreadAbortException)
                {
                    return;
                }
                catch (Exception)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                }
            }
            while (true);
        }

        /// <summary>
        /// Sets state of downloader
        /// </summary>
        /// <param name="stateToSet">State of downloader</param>
        public void SetState(IDownloaderState stateToSet)
        {
            if (stateToSet != null)
            {
                State = stateToSet;
                NotifyPropertyChanged("State");
            }
        }

        /// <summary>
        /// start prepeare fow download
        /// startDownloadThread method of state starting
        /// </summary>
        public void StartToPrepare()
        {
            mainThread = new Thread(new ParameterizedThreadStart(State.StartDownloadThread));
            mainThread.Start(RequestedSegments);
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

            NotifyPropertyChanged("LocalFile");
            List<CalculatedFileSegment> calculatedSegments;

            if (!remoteFileInfo.AcceptRanges)
            {
                calculatedSegments = new List<CalculatedFileSegment> { new CalculatedFileSegment(0, remoteFileInfo.FileSize) };
            }
            else
            {
                calculatedSegments = this.SegmentCalculator.GetSegments(segmentCount, remoteFileInfo);
            }

            if (threads != null)
            {
                lock (threads)
                {
                    threads.Clear();
                }
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

            NotifyPropertyChanged("Segment");

            RunSegments();
        }

        public bool AllWorkersStopped(int timeout)
        {
            bool allFinished = true;

            List<Thread> workThreads = new List<Thread>();
            if (threads != null)
            {
                lock (threads)
                {
                    workThreads.AddRange(threads);
                }
            }

            foreach (Thread t in workThreads)
            {
                bool finished = t.Join(timeout);
                allFinished = allFinished & finished;

                if (finished && threads != null)
                {
                    lock (threads)
                    {
                        threads.Remove(t);
                    }
                }
            }

            NotifyPropertyChanged("Threads");

            return allFinished;
        }

        public void Start()
        {
            try
            {
                State.Start();
            }
            catch (Exception)
            {
                throw new InvalidOperationException();
            }
        }

        public void Pause()
        {
            try
            {
                State.Pause();
            }
            catch (Exception)
            {
                throw new InvalidOperationException();
            }
        }

        public void StartDownloadThread(object valueSegmentCount)
        {
            try
            {
                State.StartDownloadThread(valueSegmentCount);
                NotifyPropertyChanged("State");
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        private void RunSegments()
        {
            State.SetState(new DownloadDownloadingState(this));
            NotifyPropertyChanged("State");
            using (FileStream fs = new FileStream(this.LocalFile + "\\" + FileName, FileMode.Open, FileAccess.Write))
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
                    State.SetState(new DownloadEndedWithErrorState(this));
                    return;
                }
            }

            State.SetState(new DownloadEndedState(this));
            NotifyPropertyChanged("State");
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

            NotifyPropertyChanged("Segment");

            Thread.Sleep((int)delay);

            return hasErrors;
        }

        private void StartSegment(FileSegment newSegment)
        {
            Thread segmentThread = new Thread(new ParameterizedThreadStart(FileSegmentThreadProc));
            segmentThread.Start(newSegment);
            NotifyPropertyChanged("Threads");
            if (threads == null)
                threads = new List<Thread>();
            lock (threads)
            {
                threads.Add(segmentThread);
            }
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
                    State.SetState(new DownloadPreparingState(this));

                    restartTriesCounter++;
                    try
                    {
                        reloadedFileInfo = ProtocolProvider.GetFileInfo(this.ResourceInfo, out downloadStream);
                        NotifyPropertyChanged("FileInfo");
                        break;
                    }
                    catch (Exception)
                    {
                        if (restartTriesCounter < Settings.Default.MaxTries)
                        {
                            State.SetState(new DownloadWaitingForReconnectState(this));
                            NotifyPropertyChanged("State");
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
                State.SetState(new DownloadPreparedState(this));
                NotifyPropertyChanged("State");
            }

            try
            {
                // check if the file already have changed on the server
                if (!reloadedFileInfo.AcceptRanges ||
                    reloadedFileInfo.LastModified > RemoteFileInfo.LastModified ||
                    reloadedFileInfo.FileSize != RemoteFileInfo.FileSize)
                {
                    this.remoteFileInfo = reloadedFileInfo;
                    NotifyPropertyChanged("RemoteFileInfo");
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
            catch (Exception)
            {
                State.SetState(new DownloadEndedWithErrorState(this));
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
                    segment.CurrentLink = resourceInfo.Url;
                    NotifyPropertyChanged("Segment");
                }
                else
                {
                    ////  change the segment URL to the main URL
                    segment.CurrentLink = ResourceInfo.Url;
                    NotifyPropertyChanged("Segment");
                }

                ReadSegment(segment, buffSize, buffer);

            }
            catch (Exception ex)
            {
                //// store the error information
                segment.State = FileSegmentState.Error;
                segment.LastError = ex;
                NotifyPropertyChanged("Segment");
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
                        NotifyPropertyChanged("Segment");
                        if (readSize <= 0)
                        {
                            segment.StartPosition = segment.EndPosition;
                            NotifyPropertyChanged("Segment");
                            break;
                        }
                    }

                    //// locks the stream to avoid that other threads changes
                    //// the position of stream while this thread is writing into the stream
                    lock (syncObject)
                    {
                        segment.OutputStream.Position = segment.StartPosition;
                        segment.OutputStream.Write(buffer, 0, (int)readSize);
                        NotifyPropertyChanged("Segment");
                    }

                    //// increse the start position of the segment and also calculates the rate
                    segment.IncreaseStartPosition(readSize);
                    //// check if the stream has reached its end
                    if (segment.EndPosition > 0 && segment.StartPosition >= segment.EndPosition)
                    {
                        segment.StartPosition = segment.EndPosition;
                        NotifyPropertyChanged("Segment");
                        break;
                    }

                    //// check if the user have requested to pause the download
                    if (State.State == DownloadState.Pausing)
                    {
                        segment.State = FileSegmentState.Paused;
                        NotifyPropertyChanged("Segment");
                        break;
                    }

                }
                while (readSize > 0);
                if (segment.State == FileSegmentState.Downloading)
                {
                    segment.State = FileSegmentState.Finished;
                    NotifyPropertyChanged("State");

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
                        NotifyPropertyChanged("Segments");

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
                    newFileName = this.LocalFile + currentVersion.ToString(CultureInfo.CurrentCulture);
                }
                while (new FileInfo(newFileName).Exists);

                LocalFile = newFileName;
                NotifyPropertyChanged("LocalFile");
            }

            using (FileStream fs = new FileStream(this.LocalFile + "\\" + FileName, FileMode.Create, FileAccess.Write))
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
            if (Mirrors == null || mirrors.Count() == 0)
            {
                return this.ResourceInfo;
            }

            lock (Mirrors)
            {
                if (mirrorCounter >= mirrors.Count())
                {
                    mirrorCounter = 0;

                    return this.ResourceInfo;
                }

                return mirrors[mirrorCounter++];
            }
        }

        public override int GetHashCode()
        {
            return this.CreatedDateTime.GetHashCode() 
                ^ this.FileName.GetHashCode() 
                ^ this.FileSize.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Downloader);
        }

        //public override bool operator=(Downloader other)
        //{
        //    return EqualityComparer<Downloader>.Default.Equals(this, other);
        //}

        //public override bool operator!=(Downloader other)
        //{
        //    return !(this == other);
        //}

        public bool Equals(Downloader other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.FileName == other.FileName
                && this.FileSize == other.FileSize
                && this.CreatedDateTime == other.CreatedDateTime;
        }

        #endregion

        #region INotifyPropertyChanged

        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        #endregion
    }
}
