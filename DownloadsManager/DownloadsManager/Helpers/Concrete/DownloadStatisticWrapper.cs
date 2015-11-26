using DownloadsManager.Core.Concrete;
using DownloadsManager.Core.Concrete.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Helpers.Concrete
{
    /// <summary>
    /// Wrapper class for statistic representation
    /// </summary>
    public class DownloadStatisticWrapper
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="type">type of downloaded file</param>
        /// <param name="count">count of files</param>
        /// <param name="state">state of download</param>
        public DownloadStatisticWrapper(FileType type, int count, DownloadState state)
        {
            FileType = type;
            Count = count;
            State = state;
        }

        /// <summary>
        /// type of file
        /// </summary>
        public FileType FileType { get; private set; }

        /// <summary>
        /// count of files
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// state of download
        /// </summary>
        public DownloadState State { get; private set; }
    }
}
