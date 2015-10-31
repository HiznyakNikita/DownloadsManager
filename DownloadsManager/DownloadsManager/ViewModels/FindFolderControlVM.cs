using DownloadsManager.ViewModels.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.ViewModels
{
    public class FindFolderControlVM : MainVM, IFindFolderControlVM
    {
        public string FolderPath { get; set; }
        public FindFolderControlVM() { }
    }
}
