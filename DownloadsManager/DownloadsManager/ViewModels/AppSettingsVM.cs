using DownloadsManager.Properties;
using DownloadsManager.ViewModels.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.ViewModels
{
    public class AppSettingsVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public AppSettingsVM()
        {
            this.SaveSettingsCmd = new Command(SaveSettings);
        }

        public static string DefaultSavePathMusic 
        { 
            get
            {
                return Settings.Default.DefaultSavePathMusic;
            }
        }
        public static string DefaultSavePathVideo 
        { 
            get
            {
                return Settings.Default.DefaultSavePathVideo;
            }
        }
        public static string DefaultSavePathPictures 
        { 
            get
            {
                return Settings.Default.DefaultSavePathPictures;
            }
        }
        public static string DefaultSavePathApps 
        { 
            get
            {
                return Settings.Default.DefaultSavePathApps;
            }
        }
        public static string DefaultSavePathDocuments 
        { 
            get
            {
                return Settings.Default.DefaultSavePathDocuments;
            }
        }

        public static bool CleanEndedDownloads 
        { 
            get
            {
                return Settings.Default.CleanEndedDownloads;
            }
        }

        public Command SaveSettingsCmd { get; set; }

        private void SaveSettings(object param)
        {
            Settings.Default.CleanEndedDownloads = CleanEndedDownloads;
            Settings.Default.DefaultSavePathApps = DefaultSavePathApps;
            Settings.Default.DefaultSavePathDocuments = DefaultSavePathDocuments;
            Settings.Default.DefaultSavePathMusic = DefaultSavePathMusic;
            Settings.Default.DefaultSavePathPictures = DefaultSavePathPictures;
            Settings.Default.DefaultSavePathVideo = DefaultSavePathVideo;
            
            NotifyPropertyChanged("DefaultSavePathApps");
            NotifyPropertyChanged("DefaultSavePathDocuments");
            NotifyPropertyChanged("DefaultSavePathMusic");
            NotifyPropertyChanged("DefaultSavePathVideo");
            NotifyPropertyChanged("DefaultSavePathPictures");
            NotifyPropertyChanged("CleanEndedDownloads");
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
