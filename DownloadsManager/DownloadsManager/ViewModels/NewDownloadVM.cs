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
        private List<ResourceInfo> mirrors = new List<ResourceInfo>();
        private ResourceInfo mirror;
        private string savePath;
        private int segmentsCount;

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

        public void AddMirrorToList(string mirrorToAdd)
        {
            ResourceInfo ri = new ResourceInfo();
            ri.URL = mirrorToAdd;
            mirrors.Add(ri);
            NotifyPropertyChanged("Mirrors");
        }

        public void RemoveMirrorFromList(string mirrorToAdd)
        {
            ResourceInfo mirrorToDelete = mirrors.Where(m => m.URL == mirrorToAdd).FirstOrDefault();
            if (mirrorToDelete != null)
            {
                mirrors.Remove(mirrorToDelete);
                NotifyPropertyChanged("Mirrors");
            }
        }

        public void AddMirror(string mirrorToAdd)
        {
            ResourceInfo ri = new ResourceInfo();
            ri.URL = mirrorToAdd;
            this.mirror = ri;
            NotifyPropertyChanged("Mirror");
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
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
