using DownloadsManager.Core.Abstract;
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

        /// <summary>
        /// provider for this downloader
        /// </summary>
        private IProtocolProvider defaultDownloadProvider;

        /// <summary>
        /// loacalFile
        /// </summary>
        private string localFile;

        /// <summary>
        /// createdDateTime
        /// </summary>
        private DateTime createdDateTime;
        
        /// <summary>
        /// to show info about file
        /// </summary>
        private RemoteFileInfo remoteFileInfo;

        /// <summary>
        /// info about remote resource
        /// </summary>
        private ResourceInfo resourceInfo;
        
        /// <summary>
        /// status of downloading
        /// </summary>
        private string statusMessage;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="localFile">local file</param>
        /// <param name="remoteInfo">remote info</param>
        /// <param name="ri">resource info</param>
        /// <param name="createdDateTime">created date time</param>
        public Downloader(string localFile, RemoteFileInfo remoteInfo, ResourceInfo ri, DateTime createdDateTime)
        {
            this.localFile = localFile;
            this.remoteFileInfo = remoteInfo;
            this.createdDateTime = createdDateTime;
            this.resourceInfo = ri;
            defaultDownloadProvider = ri.BindProtocolProviderInstance();
        }

        #region Properties

        /// <summary>
        /// Gets file size
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
        /// Gets date time of created
        /// </summary>
        public DateTime CreatedDateTime
        {
            get
            {
                return createdDateTime;
            }
        }

        /// <summary>
        /// Gets loal file
        /// </summary>
        public string LocalFile
        {
            get
            {
                return this.localFile;
            }
        }

        /// <summary>
        /// Gets progress status
        /// </summary>
        public double Progress
        {
            get
            {
                //TODO
                return 0;
            }
        }

        /// <summary>
        /// Gets rate of download
        /// </summary>
        public double Rate
        {
            get
            {
                //TODO
                double rate = 0;

                return rate;
            }
        }

        /// <summary>
        /// Gets time left
        /// </summary>
        public TimeSpan TimeLeft
        {
            get
            {
                //TODO
                return new TimeSpan();
            }
        }

        /// <summary>
        /// Gets or sets status message
        /// </summary>
        public string StatusMessage
        {

            get { return statusMessage; }
            set { statusMessage = value; }

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
                    remoteFileInfo = defaultDownloadProvider.GetFileInfo(this.resourceInfo);
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
