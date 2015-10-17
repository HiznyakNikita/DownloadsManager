using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Core.Concrete
{
    public enum FileSegmentState
    {
        Connecting,
        Downloading,
        Paused,
        Finished,
        Error
    }
}
