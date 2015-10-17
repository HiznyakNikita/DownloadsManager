using DownloadsManager.Core.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Core.Concrete
{
    /// <summary>
    /// Class for implementing chunked download
    /// Represent segment of file for downloading
    /// </summary>
    public class FileSegment : INotifyPropertyChanged
    {
        private long startPosition;
        private long endPosition;
        private int index;
        private string currentLink;
        private long initialStartPosition;
        private Stream outputStream;
        private Stream inputStream;
        private Exception lastError;
        private DateTime lastErrorTime = DateTime.MinValue;
        private FileSegmentState state;
        private bool isStarted = false;
        private DateTime lastSegmentReceptionTime = DateTime.MinValue;
        private double rate;
        private long start;
        private TimeSpan timeLeft = TimeSpan.Zero;
        private int currentTry;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets count of tries to restart downloads if downloading failed
        /// </summary>
        public int CurrentTry
        {
            get { return currentTry; }
            set { currentTry = value; }
        }

        /// <summary>
        /// Gets or sets state of file segment
        /// </summary>
        public FileSegmentState State
        {
            get
            {
                return state;
            }

            set
            {
                state = value;

                switch (state)
                {
                    case FileSegmentState.Downloading:
                        BeginWork();
                        break;

                    //// reset rate and timeleft fields
                    case FileSegmentState.Connecting:
                    case FileSegmentState.Paused:
                    case FileSegmentState.Finished:
                    case FileSegmentState.Error:
                        rate = 0.0;
                        timeLeft = TimeSpan.Zero;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets index of segment
        /// </summary>
        public int Index 
        {
            get
            {
                return index;
            }

            set
            {
                index = value;
            }
        }

        /// <summary>
        /// Gets time of last error
        /// </summary>
        public DateTime LastErrorTime 
        { 
            get
            {
                return lastErrorTime;
            }
        }

        /// <summary>
        /// Gets or sets last error while downloading
        /// </summary>
        public Exception LastError
        {
            get
            {
                return lastError;
            }

            set
            {
                lastError = value;
            }
        }

        /// <summary>
        /// Gets or sets initial start downloading position for file segment
        /// </summary>
        public long InitialStartPosition
        {
            get
            {
                return initialStartPosition;
            }

            set
            {
                initialStartPosition = value;
            }
        }

        /// <summary>
        /// Gets or sets start position of segment (Content-Range: bytes start-end in HTTP) 
        /// </summary>
        public long StartPosition
        {
            get
            {
                return startPosition;
            }

            set
            {
                startPosition = value;
            }
        }

        /// <summary>
        /// Gets countof transfered bytes
        /// </summary>
        public long TransferBytes
        {
            get
            {
                return this.StartPosition - this.InitialStartPosition;
            }
        }

        /// <summary>
        /// Gets calculation count of bytes which are total to transfer
        /// </summary>
        public long TotalToTransfer
        {
            get
            {
                return this.EndPosition <= 0 ? 0 : this.EndPosition - this.InitialStartPosition;
            }
        }

        /// <summary>
        /// Gets missing bytes count
        /// </summary>
        public long MissingTransfer
        {
            get
            {
                return this.EndPosition <= 0 ? 0 : this.EndPosition - this.StartPosition;
            }
        }

        /// <summary>
        /// Gets total download progress
        /// </summary>
        public double Progress
        {
            get
            {
                return this.EndPosition <= 0 ? 0 : ((double)TransferBytes / (double)TotalToTransfer * 100.0f);
            }
        }

        /// <summary>
        /// Gets or sets end position of file segment (Content-Range: bytes start-end in HTTP)
        /// </summary>
        public long EndPosition
        {
            get
            {
                return endPosition;
            }

            set
            {
                endPosition = value;
            }
        }

        /// <summary>
        /// Gets or sets file segment output stream
        /// </summary>
        public Stream OutputStream
        {
            get
            {
                return outputStream;
            }

            set
            {
                outputStream = value;
            }
        }

        /// <summary>
        /// Gets or sets file segment input stream
        /// </summary>
        public Stream InputStream
        {
            get
            {
                return inputStream;
            }

            set
            {
                inputStream = value;
            }
        }

        /// <summary>
        /// Gets or sets current file segment URL
        /// </summary>
        public string CurrentLink
        {
            get
            {
                return currentLink;
            }

            set
            {
                currentLink = value;
            }
        }

        /// <summary>
        /// Gets rate of downloading
        /// </summary>
        public double Rate
        {
            get
            {
                if (this.State == FileSegmentState.Downloading)
                {
                    IncreaseStartPosition(0);
                    return rate;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Gets time left for downloading
        /// </summary>
        public TimeSpan TimeLeft
        {
            get
            {
                return timeLeft;
            }
        }

        /// <summary>
        /// Start working on file segment
        /// </summary>
        public void BeginWork()
        {
            start = startPosition;
            lastSegmentReceptionTime = DateTime.Now;
            isStarted = true;

            NotifyPropertyChanged("Start");
        }

        /// <summary>
        /// Increment start file segment position
        /// </summary>
        /// <param name="size">size for incrementing</param>
        public void IncreaseStartPosition(long size)
        {
            lock (this)
            {
                DateTime now = DateTime.Now;

                startPosition += size;

                if (isStarted)
                {
                    TimeSpan ts = now - lastSegmentReceptionTime;
                    if (ts.TotalSeconds == 0)
                    {
                        return;
                    }

                    // calculate bytes per seconds
                    rate = ((double)(startPosition - start)) / ts.TotalSeconds;

                    if (rate > 0.0)
                    {
                        timeLeft = TimeSpan.FromSeconds(MissingTransfer / rate);
                    }
                    else
                    {
                        timeLeft = TimeSpan.MaxValue;
                    }
                }
                else
                {
                    start = startPosition;
                    lastSegmentReceptionTime = now;
                    isStarted = true;
                }

                NotifyPropertyChanged("Start");
            }
        }

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
