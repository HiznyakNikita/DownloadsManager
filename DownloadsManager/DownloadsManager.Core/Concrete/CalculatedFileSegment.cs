using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DownloadsManager.Core.Concrete
{
    public class CalculatedFileSegment
    {
        private long segmentStartPosition;
        private long segmentEndPosition;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="startPosition">start segment position</param>
        /// <param name="endPosition">end segment position</param>
        public CalculatedFileSegment(long startPosition, long endPosition)
        {
            this.segmentEndPosition = endPosition;
            this.segmentStartPosition = startPosition;
        }

        /// <summary>
        /// Gets start segment position
        /// </summary>
        public long SegmentStartPosition
        {
            get { return segmentStartPosition; }
        }

        /// <summary>
        /// Gets end segment position
        /// </summary>
        public long SegmentEndPosition
        {
            get { return segmentEndPosition; }
        }
    }
}
