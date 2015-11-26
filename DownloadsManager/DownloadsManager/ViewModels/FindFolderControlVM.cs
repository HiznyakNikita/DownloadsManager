using DownloadsManager.ViewModels.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.ViewModels
{
    /// <summary>
    /// VM fro find folder control
    /// </summary>
    public class FindFolderControlVM : MainVM, IFindFolderControlVM
    {
        public FindFolderControlVM() 
        { 

        }

        public string FolderPath { get; set; }
    }
}
