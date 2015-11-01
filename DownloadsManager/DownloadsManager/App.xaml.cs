using Autofac;
using DownloadsManager.ViewModels;
using DownloadsManager.ViewModels.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using DownloadsManager.Helpers;
using Autofac.Core;

namespace DownloadsManager
{
    [CLSCompliant(true)]

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initialize IoC container
        /// </summary>
        /// <param name="e">events</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            AutofacHelper.OnStartupInit();
            IWindowOpener opener = AutofacHelper.Container.Resolve<IWindowOpener>();
            opener.OpenNewWindow(AutofacHelper.Container.Resolve<IMainWindowVM>());
        }
    }
}
