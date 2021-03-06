﻿using DownloadsManager.Properties;
using DownloadsManager.ViewModels.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.ViewModels
{
    /// <summary>
    /// View model for AppSettings window
    /// </summary>
    public class AppSettingsVM : MainVM, IAppSettingsVM
    {
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

        //save settings  for DM
        private void SaveSettings()
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
    }
}
