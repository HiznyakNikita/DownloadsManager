using DownloadsManager.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DownloadsManager.Views
{
    /// <summary>
    /// Interaction logic for AppSettingsView.xaml
    /// </summary>
    public partial class AppSettingsView : Window, INotifyPropertyChanged
    {
        private AppSettingsVM model;

        public AppSettingsView()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets VM of view
        /// </summary>
        public AppSettingsVM Model
        {
            get
            {
                return model;
            }

            set
            {
                model = value;
                NotifyPropertyChanged("Model");
            }
        }

        private void BtnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnChooseMusicFolderSaveTo_Click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = folderBrowserDialog1.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    tbSaveMusicPathTo.Text = folderBrowserDialog1.SelectedPath;
                }
            }
        }

        private void BtnChoosePictureFolderSaveTo_Click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = folderBrowserDialog1.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    tbSavePictureToPath.Text = folderBrowserDialog1.SelectedPath;
                }
            }
        }

        private void BtnChooseVideoFolderSaveTo_Click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = folderBrowserDialog1.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    tbSaveVideoToPath.Text = folderBrowserDialog1.SelectedPath;
                }
            }
        }

        private void BtnChooseDocumentsFolderSaveTo_Click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = folderBrowserDialog1.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    tbSaveDocumentsToPath.Text = folderBrowserDialog1.SelectedPath;
                }
            }
        }

        private void BtnChooseAppFolderSaveTo_Click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = folderBrowserDialog1.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    tbSaveAppToPath.Text = folderBrowserDialog1.SelectedPath;
                }
            }
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
