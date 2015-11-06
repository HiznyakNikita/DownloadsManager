using DownloadsManager.ViewModels;
using DownloadsManager.ViewModels.Infrastructure;
using DownloadsManager.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DownloadsManager.Helpers
{
    /// <summary>
    /// Opener interface
    /// </summary>
    public interface IWindowOpener
    {
        /// <summary>
        /// method for open new window and attach VM
        /// </summary>
        /// <param name="model">view model</param>
        void OpenNewWindow(IMainVM model);
    }
}
