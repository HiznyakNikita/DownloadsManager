﻿using DownloadsManager.Core.Abstract;
using DownloadsManager.Core.Concrete.DownloadStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Core.Concrete.DownloadFilters
{
    /// <summary>
    /// Filter which match failed downloads
    /// </summary>
    public class FailedDownloadsFilter : IFilter<Downloader>
    {
        public bool IsSuitable(Downloader download)
        {
            return download.State.GetType() == typeof(DownloadEndedWithErrorState);
        }
    }
}