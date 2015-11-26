using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.ViewModels.Infrastructure
{
    /// <summary>
    /// Interface for MainWindowVM
    /// </summary>
    public interface IMainWindowVM : IMainVM
    {
        Hashtable GetDownloaders();
        void AddDownloadFromArgs();
    }
}
