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
        private string localFile;
        private DateTime createdDateTime;
        //used to show information about file
        private RemoteFileInfo remoteFileInfo;
        private string statusMessage;

        public Downloader(string localFile, RemoteFileInfo remoteInfo, DateTime createdDateTime)
        {
            this.localFile = localFile;
            this.remoteFileInfo = remoteInfo;
            this.createdDateTime = createdDateTime;
        }
        #region Properties

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

        public DateTime CreatedDateTime
        {
            get
            {
                return createdDateTime;
            }
        }

        public string LocalFile
        {
            get
            {
                return this.localFile;
            }
        }

        public double Progress
        {
            get
            {
               //TODO
                return 0;
            }
        }

        public double Rate
        {
            get
            {
                //TODO
                double rate = 0;

                return rate;
            }
        }

        public TimeSpan TimeLeft
        {
            get
            {
                //TODO
                return new TimeSpan();
            }
        }

        public string StatusMessage
        {
            get { return statusMessage; }
            set { statusMessage = value; }
        }


        #endregion

        public void Download(string uri, string path)
        {
            WebClient client = new WebClient();
            // Hookup DownloadFileCompleted Event
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleted);

            client.DownloadFileAsync(new Uri(uri), path);
        }

        private void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Debug.WriteLine("Download completed");
        }
    }
}
