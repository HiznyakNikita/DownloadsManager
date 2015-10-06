using DownloadsManager.Core.Abstract;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Core.Concrete
{
    /// <summary>
    /// Implementation filter iterator for downloads
    /// </summary>
    public class DownloadsIterator : IEnumerator<Downloader>
    {
        private DownloadsList instance;
        private int currentPosition;
        private IFilter<Downloader> filter;

        public DownloadsIterator(DownloadsList list, IFilter<Downloader> filter)
        {
            this.instance = list;
            this.filter = filter;
        }

        object IEnumerator.Current { get { return Current; } }

        public Downloader Current
        {
            get
            {
                if (0 <= currentPosition && currentPosition < instance.Count)
                {
                    return instance[currentPosition];
                }
                else
                    return null;
            }
        }

        void IDisposable.Dispose() { }

        public bool MoveNext() 
        { 
            for (int i = currentPosition + 1; i < instance.Count; i++)
            {
                if (filter.IsSuitable(instance[i])) { return true; }
            }

            return false;
        }

        public void Reset() 
        {
            currentPosition = 0;
        }
    }
}
