using DownloadsManager.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Core.Concrete
{
    public class DownloadsList : List<Downloader>
    {
        public DownloadsIterator GetIterator(IFilter<Downloader> filter)
        {
            return new DownloadsIterator(filter);
        }
    }
}
