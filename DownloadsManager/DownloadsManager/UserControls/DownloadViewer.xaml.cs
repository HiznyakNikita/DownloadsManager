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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DownloadsManager.ViewModels;
using DownloadsManager.Views;

namespace DownloadsManager.UserControls
{
    /// <summary>
    /// Interaction logic for DownloadViewer.xaml
    /// </summary>
    public partial class DownloadViewer : UserControl
    {
        /// <summary>
        /// ctor
        /// </summary>
        public DownloadViewer()
        {
            InitializeComponent();
        }

        private void btnAdditionalInfo_Click(object sender, RoutedEventArgs e)
        {
            SegmentsInfoView segmentsInfoView = new SegmentsInfoView(((DownloadViewerVM)this.DataContext).Download);
            segmentsInfoView.ShowDialog();
        }
    }
}
