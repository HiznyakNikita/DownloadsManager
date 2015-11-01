using DownloadsManager.Core.Concrete;
using DownloadsManager.Core.Properties;
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
    [Serializable]
    public abstract class FileSegmentCalculator
    {
        public virtual List<CalculatedFileSegment> GetSegments(int segmentCount, RemoteFileInfo remoteFileInfo)
        {
            long calculatedSegmentSize = CalculateSegmentSize(segmentCount, remoteFileInfo);
            long residueBytes = CalculateResidueBytes(segmentCount, remoteFileInfo, calculatedSegmentSize);
            return GetCalculatedSegments(segmentCount, remoteFileInfo, calculatedSegmentSize, residueBytes);
        }

        public virtual long CalculateSegmentSize(int segmentCount, RemoteFileInfo remoteFileInfo)
        {
            if (remoteFileInfo != null)
            {
                long calculatedSegmentSize = remoteFileInfo.FileSize / (long)segmentCount;
                return calculatedSegmentSize;
            }
            
            return 0;
        }

        public virtual long CalculateResidueBytes(int segmentCount, RemoteFileInfo remoteFileInfo, long calculatedSegmentSize)
        {
            return 0;
        }

        public virtual List<CalculatedFileSegment> GetCalculatedSegments(
            int segmentCount, 
            RemoteFileInfo remoteFileInfo, 
            long calculatedSegmentSize, 
            long residueBytes)
        {
            List<CalculatedFileSegment> calculatedSegments = new List<CalculatedFileSegment>();
            if (remoteFileInfo != null)
            {
                long startSegmentPosition = 0;
                for (int i = 0; i < segmentCount; i++)
                {
                    if (i != segmentCount - 1)
                    {
                        if (segmentCount == 1)
                        {
                            calculatedSegments.Add(new CalculatedFileSegment(startSegmentPosition, remoteFileInfo.FileSize));
                        }
                        else
                        {
                            calculatedSegments.Add(new CalculatedFileSegment(
                                startSegmentPosition,
                                startSegmentPosition + calculatedSegmentSize));
                        }
                    }

                    if (i == segmentCount - 1)
                    {
                        calculatedSegments.Add(new CalculatedFileSegment(
                            startSegmentPosition,
                            startSegmentPosition + calculatedSegmentSize + residueBytes));
                    }

                    startSegmentPosition = calculatedSegments[calculatedSegments.Count - 1].SegmentEndPosition;
                }
            }
            return calculatedSegments;
        }
    }
}
