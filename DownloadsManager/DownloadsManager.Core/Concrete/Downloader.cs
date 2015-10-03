using DownloadsManager.Core.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
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
        /// status of downloading
        /// </summary>
        private string statusMessage;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="localFile">local file</param>
        /// <param name="remoteInfo">remote info</param>
        /// <param name="createdDateTime">created date time</param>
        public Downloader(string localFile, RemoteFileInfo remoteInfo, DateTime createdDateTime)
        {
            this.localFile = localFile;
            this.remoteFileInfo = remoteInfo;
            this.createdDateTime = createdDateTime;
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
            WebClient client = new WebClient();

            //// Hookup DownloadFileCompleted Event
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleted);

            client.DownloadFileAsync(new Uri(uri), path);
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
