using DownloadsManager.Core.Abstract;
using DownloadsManager.Core.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Core.Concrete
{
    [Serializable]
    public class FileSegmentSizeCalculatorWithResidueHelper : FileSegmentCalculator
    {
        /// <summary>
        /// Method for calculating minimum size for file segment to download
        /// </summary>
        /// <param name="segmentCount">count of segments</param>
        /// <param name="remoteFileInfo">remote file information</param>
        /// <returns>List of calulated file segments</returns>
        public override List<CalculatedFileSegment> GetSegments(int segmentCount, RemoteFileInfo remoteFileInfo)
        {
            List<CalculatedFileSegment> calculatedSegments = new List<CalculatedFileSegment>();
            if (remoteFileInfo != null)
            {
                long calculatedSegmentSize = CalculateSegmentSize(segmentCount, remoteFileInfo);
                long residueBytes = CalculateResidueBytes(segmentCount, remoteFileInfo, calculatedSegmentSize);
                calculatedSegments = GetCalculatedSegments(segmentCount, remoteFileInfo, calculatedSegmentSize, residueBytes);
            }

            return calculatedSegments;
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

        public override long CalculateSegmentSize(int segmentCount, RemoteFileInfo remoteFileInfo)
        {
            return base.CalculateSegmentSize(segmentCount, remoteFileInfo);
        }

        public override List<CalculatedFileSegment> GetCalculatedSegments(int segmentCount, RemoteFileInfo remoteFileInfo, long calculatedSegmentSize, long residueBytes)
        {
            return base.GetCalculatedSegments(segmentCount, remoteFileInfo, calculatedSegmentSize, residueBytes);
        }
    }
}
