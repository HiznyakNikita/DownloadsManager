using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Core.Concrete
{
    public class SegmentsIterator : IEnumerator<FileSegment>
    {
        private Downloader instance;
        private int currentPosition;

        public SegmentsIterator(Downloader list)
        {
            this.instance = list;
        }

        object IEnumerator.Current { get { return Current; } }

        public FileSegment Current
        {
            get
            {
                if (0 <= currentPosition && currentPosition < instance.FileSegments.Count)
                {
                    return instance.FileSegments[currentPosition];
                }
                else
                    return null;
            }
        }

        void IDisposable.Dispose() { }

        public bool MoveNext() 
        { 
            if(currentPosition+1 < instance.FileSegments.Count)
            {
                currentPosition++;
                return true; 
            }

            return false;
        }

        public void Reset() 
        {
            currentPosition = 0;
        }
    }
}
