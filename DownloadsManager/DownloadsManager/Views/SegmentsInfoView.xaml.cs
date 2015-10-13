using DownloadsManager.Core.Concrete;
using DownloadsManager.ViewModels;
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
    /// Interaction logic for SegmentsInfoView.xaml
    /// </summary>
    public partial class SegmentsInfoView : Window
    {
        public SegmentsInfoView(Downloader download)
        {
            InitializeComponent();
            this.DataContext = new SegmentsInfoVM(download);
        }

        private void BtnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
