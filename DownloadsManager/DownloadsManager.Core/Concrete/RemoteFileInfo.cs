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
        private long fileSize;
        private DateTime lastModified = DateTime.MinValue;

        public long FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }

        public DateTime LastModified
        {
            get { return lastModified; }
            set { lastModified = value; }
        }
    }
}
