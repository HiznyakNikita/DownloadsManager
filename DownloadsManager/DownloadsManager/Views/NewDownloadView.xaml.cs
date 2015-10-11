using System;
using System.Collections.Generic;
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
    public partial class NewDownloadView : Window
    {
        private string urlToDownload;

        /// <summary>
        /// ctor
        /// </summary>
        public NewDownloadView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Uri for download
        /// </summary>
        public Uri UrlToDownload
        {
            get
            {
                return new Uri(urlToDownload);
            }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            urlToDownload = tbUrlToDownload.Text;
            this.Close();
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnChooseFolderSaveTo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
