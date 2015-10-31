﻿using DownloadsManager.ViewModels;
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
    /// Window opener
    /// </summary>
    public class WindowOpener : IWindowOpener
    {
        public void OpenNewWindow(IMainVM model)
        {
            if(model is IMainWindowVM)
                using (var container = AutofacHelper.Container)
                {
                    var newWindow = new MainView((IMainWindowVM)model);
                    newWindow.ShowDialog();
                }

            if (model is INewDownloadVM)
                using (var container = AutofacHelper.Container)
                {
                    var newWindow = new NewDownloadView((INewDownloadVM)model);
                    newWindow.ShowDialog();
                }

            if (model is IAppSettingsVM)
                using (var container = AutofacHelper.Container)
                {
                    var newWindow = new AppSettingsView((IAppSettingsVM)model);
                    newWindow.ShowDialog();
                }
        }
    }
}
