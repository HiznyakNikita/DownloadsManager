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
    public class FileSegmentSizeCalculatorHelper : IFileSegmentCalculator
    {
        /// <summary>
        /// Method for calculating minimum size for file segment to download
        /// </summary>
        /// <param name="segmentCount">count of segments</param>
        /// <param name="remoteFileInfo">remote file information</param>
        /// <returns>List of calulated file segments</returns>
        public List<CalculatedFileSegment> GetSegments(int segmentCount, RemoteFileInfo remoteFileInfo)
        {
            List<CalculatedFileSegment> calculatedSegments = new List<CalculatedFileSegment>();
            if (remoteFileInfo != null)
            {
                long minSegmentSize = Settings.Default.MinSegmentSize;
                long calculatedSegmentSize = remoteFileInfo.FileSize / (long)segmentCount;
                ////optimize segment size if possible
                while (calculatedSegmentSize < minSegmentSize && segmentCount > 1)
                {
                    segmentCount--;
                    calculatedSegmentSize = remoteFileInfo.FileSize / (long)segmentCount;
                }
                ////check for residue after dividing
                long residueBytes = remoteFileInfo.FileSize - (calculatedSegmentSize * segmentCount);
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
