using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Core.Concrete.InternetExplorerExtension
{
    public static class RegistryHelper
    {
        public static void AddNewRegistryKey(Guid extensionPathValue)
        {
            Microsoft.Win32.RegistryKey key;
            key = Microsoft.Win32.Registry.LocalMachine
                .CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Extensions\" + extensionPathValue.ToString());
            key.Close();
        }

        public static void SetRegistryKeyValues(Guid extensionPathValue)
        {
            Microsoft.Win32.RegistryKey key;
            key = Microsoft.Win32.Registry.LocalMachine
                .CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Extensions\{"
                + extensionPathValue.ToString().ToUpper(CultureInfo.CurrentCulture) + "}");
            key.SetValue("ButtonText", "Download via Download manager");
            key.SetValue("HotIcon",@"C:\Users\nikit_000\Downloads\1.ico");
            key.SetValue("Icon", @"C:\Users\nikit_000\Downloads\1.ico");
            key.SetValue("CLSID", "{1FBA04EE-3024-11D2-8F1F-0000F87ABD16}");
            key.SetValue("Exec", @"D:\C#\DownloadsManager\DownloadsManager\DownloadsManager\bin\Debug\DownloadsManager.exe");
            key.SetValue("Default Visible", "Yes");
            key.Close();
        }
    }
}
