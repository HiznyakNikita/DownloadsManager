﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.ViewModels.Infrastructure
{
    public interface IMainWindowVM : IMainVM
    {
        Hashtable GetDownloaders();
        void AddDownloadFromArgs();
    }
}
