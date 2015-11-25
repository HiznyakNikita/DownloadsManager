using DownloadsManager.ViewModels;
using DownloadsManager.ViewModels.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
    /// Interaction logic for NewDownloadView.xaml
    /// </summary>
    public partial class NewDownloadView : Window, INotifyPropertyChanged
    {
        private INewDownloadVM _model;

        public NewDownloadView(INewDownloadVM model)
        {
            InitializeComponent();
            this.DataContext = _model = model;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets VM of view
        /// </summary>
        public INewDownloadVM Model 
        { 
            get
            {
                return _model;
            }

            set
            {
                _model = value;
                NotifyPropertyChanged("Model");
            }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(tbUrlToDownload.Text.ToString()))
                {
                    if (!string.IsNullOrEmpty(tbUrlToDownload.Text.ToString()))
                    {
                        _model.AddMirror(tbUrlToDownload.Text);
                    }

                    _model.AddSavePath(string.IsNullOrEmpty(tbSaveToPath.Text) ? string.Empty : tbSaveToPath.Text);

                    _model.AddDownload();
                    this.Close();
                }
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Error!");
            }
        }

        //private void BtnRemove_Click(object sender, RoutedEventArgs e)
        //{
        //    if (lstBoxAlternativeUrl.SelectedItem != null)
        //    {
        //        _model.RemoveMirrorFromList(lstBoxAlternativeUrl.SelectedValue.ToString());
        //    }
        //}

        //private void BtnAdd_Click(object sender, RoutedEventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(tbUrlToDownloadAlternative.Text))
        //    {
        //        _model.AddMirrorToList(tbUrlToDownloadAlternative.Text);
        //    }
        //}

        private void BtnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnChooseFolderSaveTo_Click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = folderBrowserDialog1.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    tbSaveToPath.Text = folderBrowserDialog1.SelectedPath;
                }
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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

        //private void CheckBoxAlternative_Checked(object sender, RoutedEventArgs e)
        //{
        //    if (checkBoxAlternative.IsChecked == true)
        //    {
        //        lblUrl.Visibility = Visibility.Visible;
        //        btnAddUrl.IsEnabled = true;
        //        btnRemoveUrl.IsEnabled = true;
        //        tbUrlToDownloadAlternative.IsEnabled = true;
        //        lstBoxAlternativeUrl.IsEnabled = true;
        //    }
        //    else
        //    {
        //        lblUrl.Visibility = Visibility.Hidden;
        //        btnAddUrl.IsEnabled = false;
        //        btnRemoveUrl.IsEnabled = false;
        //        tbUrlToDownloadAlternative.IsEnabled = false;
        //        lstBoxAlternativeUrl.IsEnabled = false;
        //    }
        //}
    }
}
