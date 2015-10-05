using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Core.Concrete.Enums
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
