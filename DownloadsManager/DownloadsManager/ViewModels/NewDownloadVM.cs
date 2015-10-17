using DownloadsManager.Core.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.ViewModels
{
    public class NewDownloadVM : INotifyPropertyChanged
    {
        private readonly List<ResourceInfo> mirrors = new List<ResourceInfo>();
        private ResourceInfo mirror;
        private string savePath;
        private int segmentsCount;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets count of download segments
        /// </summary>
        public int SegmentsCount
        { 
            get
            {
                return segmentsCount;
            }

            set
            {
                segmentsCount = value;
            }
        }

        /// <summary>
        /// Gets mirrors for downloading
        /// </summary>
        public IList<ResourceInfo> Mirrors 
        { 
            get
            {
                return mirrors;
            }
        }

        /// <summary>
        /// Gets mirror for downloading
        /// </summary>
        public ResourceInfo Mirror 
        { 
            get
            {
                return mirror;
            }
        }

        /// <summary>
        /// Gets or sets path for saving download
        /// </summary>
        public string SavePath 
        { 
            get
            {
                return savePath;
            }

            set
            {
                savePath = value;
                NotifyPropertyChanged("SavePath");
            }
        }

        /// <summary>
        /// Adding mirror to mirror list of VM
        /// </summary>
        /// <param name="mirrorToAdd">mirror to add</param>
        public void AddMirrorToList(string mirrorToAdd)
        {
            ResourceInfo ri = new ResourceInfo();
            ri.Url = mirrorToAdd;
            mirrors.Add(ri);
            NotifyPropertyChanged("Mirrors");
        }

        /// <summary>
        /// Removing mirror from list
        /// </summary>
        /// <param name="mirrorToRemove">mirror to remove</param>
        public void RemoveMirrorFromList(string mirrorToRemove)
        {
            ResourceInfo mirrorToDelete = mirrors.Where(m => m.Url == mirrorToRemove).FirstOrDefault();
            if (mirrorToDelete != null)
            {
                mirrors.Remove(mirrorToDelete);
                NotifyPropertyChanged("Mirrors");
            }
        }

        /// <summary>
        /// Add one mirror to VM
        /// </summary>
        /// <param name="mirrorToAdd">miror to add</param>
        public void AddMirror(string mirrorToAdd)
        {
            ResourceInfo ri = new ResourceInfo();
            ri.Url = mirrorToAdd;
            this.mirror = ri;
            NotifyPropertyChanged("Mirror");
        }

        #region INotifyPropertyChanged

        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        #endregion
    }
}
