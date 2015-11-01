using DownloadsManager.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Core.Concrete.Helpers
{
    /// <summary>
    /// Segment calculator with optimizing size and calculating residue
    /// </summary>
    [Serializable]
    public class FileSegmentSizeCalculatorForSegmentCountHelper : FileSegmentCalculator
    {
        public override List<CalculatedFileSegment> GetSegments(int segmentCount, RemoteFileInfo remoteFileInfo)
        {
            return base.GetSegments(segmentCount, remoteFileInfo);
        }

        public override long CalculateSegmentSize(int segmentCount, RemoteFileInfo remoteFileInfo)
        {
            return base.CalculateSegmentSize(segmentCount, remoteFileInfo);
        }

        public override long CalculateResidueBytes(int segmentCount, RemoteFileInfo remoteFileInfo, long calculatedSegmentSize)
        {
            if (remoteFileInfo != null)
            {
                return remoteFileInfo.FileSize - (calculatedSegmentSize * segmentCount);
            }
            else
            {
                return 0;
            }
        }

        public override List<CalculatedFileSegment> GetCalculatedSegments(int segmentCount, RemoteFileInfo remoteFileInfo, long calculatedSegmentSize, long residueBytes)
        {
            return base.GetCalculatedSegments(segmentCount, remoteFileInfo, calculatedSegmentSize, residueBytes);
        }
    }
}
