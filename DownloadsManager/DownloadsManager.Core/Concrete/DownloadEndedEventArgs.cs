using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Core.Concrete
{
    public class DownloadEndedEventArgs: EventArgs
    {
        public string DownloadName
        {
            get;
            private set;
        }

        public DownloadEndedEventArgs(string downloadName)
        {
            this.DownloadName = downloadName;
        }
    }
}
