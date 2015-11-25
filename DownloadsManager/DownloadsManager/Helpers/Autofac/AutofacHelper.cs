using Autofac;
using DownloadsManager.Helpers.Concrete;
using DownloadsManager.UserControls;
using DownloadsManager.ViewModels;
using DownloadsManager.ViewModels.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Helpers.Autofac
{
    /// <summary>
    /// Helper class for IoC
    /// </summary>
    public static class AutofacHelper
    {
        public static IContainer Container { get; private set; }

        //Add dependencies method
        public static void OnStartupInit()
        {
            var builder = new ContainerBuilder();
            builder.RegisterViewsAndViewModelsInAssembly();
            builder.RegisterType<WindowOpener>().As<IWindowOpener>().SingleInstance();
            builder.RegisterType<MainWindowVM>().As<IMainWindowVM>().InstancePerDependency();
            builder.RegisterType<AppSettingsVM>().As<IAppSettingsVM>().InstancePerDependency();
            builder.RegisterType<NewDownloadVM>().As<INewDownloadVM>().InstancePerDependency();
            builder.RegisterType<FindFolderControlVM>().As<IFindFolderControlVM>().InstancePerDependency();
            builder.RegisterType<DownloadViewerVM>().As<IDownloadViewerVM>().InstancePerDependency();

            Container = builder.Build();
        }
    }
}
