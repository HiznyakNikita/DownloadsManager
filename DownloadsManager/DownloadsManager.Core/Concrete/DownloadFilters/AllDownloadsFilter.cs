﻿using DownloadsManager.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Core.Concrete.DownloadFilters
{
    /// <summary>
    /// Filter which match all downloads
    /// </summary>
    public class AllDownloadsFilter : IFilter<Downloader>
    {
        public bool IsSuitable(Downloader download)
        {
            return true;
        }
    }
}
