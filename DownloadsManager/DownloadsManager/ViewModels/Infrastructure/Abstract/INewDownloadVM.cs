using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.ViewModels.Infrastructure
{
    /// <summary>
    /// Interface for NewDownlaodWindowVM
    /// </summary>
    public interface INewDownloadVM : IMainVM
    {
        void AddMirrorToList(string mirrorToAdd);

        void RemoveMirrorFromList(string mirrorToRemove);
        
        void AddMirror(string mirrorToAdd);
        
        void AddDownload();
        
        void AddSavePath(string path);
    }
}
