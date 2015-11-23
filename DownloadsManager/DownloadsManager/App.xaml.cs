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
using System.IO;
using System.Globalization;
using System.Management;
using System.Diagnostics;

namespace DownloadsManager
{
    [CLSCompliant(true)]

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, ISingleInstanceApp
    {

        private const string Unique = "DownloadManager Author: Hiznyak Nikita";
        // need for open current window on repeat opening program
        public static Window SingleMainWindow
        {
            get;
            set;
        }

        [STAThread]
        public static void Main(string[] args)
        {
            Settings.Default.ArgsUrl = string.Empty;

            if (args != null)
                foreach (var s in args)
                    Settings.Default.ArgsUrl = s.ToString();
            //string wmiQuery = string.Format("select CommandLine from Win32_Process where Name='{0}'", "DownloadsManager");
            //ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmiQuery);
            //ManagementObjectCollection retObjectCollection = searcher.Get();
            //foreach (ManagementObject retObject in retObjectCollection)
            //    Console.WriteLine("[{0}]", retObject["CommandLine"]);
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
            Settings.Default.ArgsUrl = string.Empty;
            if (args != null)
                foreach (var s in args)
                    Settings.Default.ArgsUrl = s.ToString();
            // Handle command line arguments of second instance
            //and open already opened main window with args from browser(if they are required)
            (SingleMainWindow as MainView).ShowWrapper();

            return true;
        }
        #endregion

        /// <summary>
        /// Initialize IoC container
        /// </summary>
        /// <param name="e">events</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            //if (e.Args != null)
            //    foreach (var s in e.Args)
            //        MessageBox.Show(s);
            //Guid IEExtensionGuid = Guid.NewGuid();
            RegistryHelper.SetRegistryKeyValues();
            AutofacHelper.OnStartupInit();
            IWindowOpener opener = AutofacHelper.Container.Resolve<IWindowOpener>();
            opener.OpenNewWindow(AutofacHelper.Container.Resolve<IMainWindowVM>());

        }
    }
}
