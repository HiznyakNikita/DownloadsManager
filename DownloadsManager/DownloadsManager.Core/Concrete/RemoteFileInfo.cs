using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Core.Concrete
{
    /// <summary>
    /// Information about remote downoloaded file
    /// </summary>
    public class RemoteFileInfo
    {
        /// <summary>
        /// filesize
        /// </summary>
        private long fileSize;

        /// <summary>
        /// lastModif
        /// </summary>
        private DateTime lastModified = DateTime.MinValue;

        /// <summary>
        /// representing ability to download part of file
        /// </summary>
        private bool acceptRanges;

        /// <summary>
        /// Gets or sets file size
        /// </summary>
        public long FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether representing ability to download part of file
        /// </summary>
        public bool AcceptRanges
        {
            get { return acceptRanges; }
            set { acceptRanges = value; }
        }

        /// <summary>
        /// Gets or sets last modif
        /// </summary>
        public DateTime LastModified
        {
            get { return lastModified; }
            set { lastModified = value; }
        }
    }
}
