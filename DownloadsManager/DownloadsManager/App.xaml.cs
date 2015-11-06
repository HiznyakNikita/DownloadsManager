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
using DownloadsManager.Properties;
using DownloadsManager.Core.Concrete.InternetExplorerExtension;

namespace DownloadsManager
{
    [CLSCompliant(true)]

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, ISingleInstanceApp
    {

        // TODO: Make this unique!
        private const string Unique = "Change this to something that uniquely identifies your program.";

        /// <summary>
        /// Initialize IoC container
        /// </summary>
        /// <param name="e">events</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            //Guid IEExtensionGuid = Guid.NewGuid();
           // RegistryHelper.AddNewRegistryKey(IEExtensionGuid);
            //RegistryHelper.SetRegistryKeyValues(IEExtensionGuid);
            AutofacHelper.OnStartupInit();
            IWindowOpener opener = AutofacHelper.Container.Resolve<IWindowOpener>();
            opener.OpenNewWindow(AutofacHelper.Container.Resolve<IMainWindowVM>());
        }

        [STAThread]
        public static void Main()
        {
            if (SingleInstance<App>.InitializeAsFirstInstance(Unique))
            {
                var application = new App();
                application.InitializeComponent();
                application.Run();

                // Allow single instance code to perform cleanup operations
                SingleInstance<App>.Cleanup();
            }
        }

        #region ISingleInstanceApp Members
        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            // Handle command line arguments of second instance
            return true;
        }
        #endregion
    }
}
