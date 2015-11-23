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
        public static void SetRegistryKeyValues()
        {
            Microsoft.Win32.RegistryKey key;
            key = Microsoft.Win32.Registry.CurrentUser
                .CreateSubKey(@"Software\Microsoft\Internet Explorer\MenuExt\Download by DM\");
            key.SetValue(
                null, 
                System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.Substring(8)) + "\\IErun.htm");
            key.SetValue("Contexts", 1);
            key.Close();
        }
    }
}
