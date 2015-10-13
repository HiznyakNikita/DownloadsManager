using DownloadsManager.Core.Concrete;
using DownloadsManager.ViewModels.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.ViewModels
{
    public class SegmentsInfoVM : INotifyPropertyChanged
    {
        private Downloader downloader;
        private SegmentsIterator segmentIterator;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="download">Download to show</param>
        public SegmentsInfoVM(Downloader download)
        {
            if(download != null)
                downloader = download;
            segmentIterator = downloader.GetSegmentsIterator();
            this.NextSegmentCmd = new Command(this.GetNextSegment);
        }

        public Command NextSegmentCmd { get; set; }

        /// <summary>
        /// Gets segment item
        /// </summary>
        public FileSegment Segment 
        {
            get
            {
                return segmentIterator.Current;
            }
        }

        /// <summary>
        /// Move to next segment
        /// </summary>
        public void GetNextSegment(object parameter)
        {
            bool res = segmentIterator.MoveNext();
            if (!res)
                segmentIterator.Reset();
            NotifyPropertyChanged("Segment");
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
