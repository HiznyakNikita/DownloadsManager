using DownloadsManager.Core.Concrete;
using DownloadsManager.Core.Concrete.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Helpers
{
    public class DownloadStatisticWrapper
    {
        public DownloadStatisticWrapper(FileType type, int count, DownloadState state)
        {
            FileType = type;
            Count = count;
            State = state;
        }
        public FileType FileType { get; private set; }
        public int Count { get; set; }
        public DownloadState State { get; private set; }
    }
}
