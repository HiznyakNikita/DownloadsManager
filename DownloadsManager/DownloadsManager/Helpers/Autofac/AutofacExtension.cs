using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Helpers.Autofac
{
    public static class AutofacExtension
    {
        /// <summary>
        /// Extension method for Registration views and VM in IoC
        /// </summary>
        /// <param name="containerBuilder">container IoC</param>
        /// <param name="assembly">current Assembly</param>
        public static void RegisterViewsAndViewModelsInAssembly(this ContainerBuilder containerBuilder, Assembly assembly = null)
        {
            containerBuilder
                .RegisterAssemblyTypes(assembly ?? Assembly.GetCallingAssembly())
                .Where(type => type.Name.EndsWith("View", StringComparison.CurrentCulture));
        }
    }
}
