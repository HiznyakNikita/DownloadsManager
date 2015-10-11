using DownloadsManager.Core.Abstract;
using DownloadsManager.Core.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Core.Concrete.Helpers
{
    public class FileSegmentSizeCalculatorHelper : IFileSegmentCalculator
    {
        /// <summary>
        /// Method for calculating minimum size for file segment to download
        /// </summary>
        /// <param name="segmentsCount">count of segments</param>
        /// <param name="ri">remote file information</param>
        /// <returns>List of calulated file segments</returns>
        public List<CalculatedFileSegment> GetSegments(int segmentsCount, RemoteFileInfo ri)
        {
            List<CalculatedFileSegment> calculatedSegments = new List<CalculatedFileSegment>();
            long minSegmentSize = Settings.Default.MinSegmentSize;
            long calculatedSegmentSize = ri.FileSize / (long)segmentsCount;
            ////optimize segment size if possible
            while (calculatedSegmentSize < minSegmentSize && segmentsCount > 1)
            {
                segmentsCount--;
                calculatedSegmentSize = ri.FileSize / (long)segmentsCount;
            }
            ////check for residue after dividing
            long residueBytes = ri.FileSize - calculatedSegmentSize * segmentsCount;
            long startSegmentPosition = 0;
            for (int i = 0; i < segmentsCount; i++)
            {
                if (i != segmentsCount - 1)
                {
                    if (segmentsCount == 1)
                    {
                        calculatedSegments.Add(new CalculatedFileSegment(startSegmentPosition, ri.FileSize));
                    }
                    else
                    {
                        calculatedSegments.Add(new CalculatedFileSegment(startSegmentPosition, startSegmentPosition + calculatedSegmentSize));
                    }
                }
                if(i==segmentsCount-1)
                {
                    calculatedSegments.Add(new CalculatedFileSegment(startSegmentPosition, startSegmentPosition + calculatedSegmentSize + residueBytes));
                }

                startSegmentPosition = calculatedSegments[calculatedSegments.Count - 1].SegmentEndPosition;
            }

            return calculatedSegments;
        }
    }
}
