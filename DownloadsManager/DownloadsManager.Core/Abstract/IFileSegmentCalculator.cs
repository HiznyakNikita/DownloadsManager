using DownloadsManager.Core.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Core.Abstract
{
    /// <summary>
    /// Insterface for calculating file segment size and optimal segments count
    /// </summary>
    public interface IFileSegmentCalculator
    {
        List<CalculatedFileSegment> GetSegments(int segmentCount, RemoteFileInfo remoteFileInfo);
    }
}
