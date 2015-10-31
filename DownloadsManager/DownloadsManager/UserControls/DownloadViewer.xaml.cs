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
using DownloadsManager.ViewModels.Infrastructure;
using DownloadsManager.UserControls.Abstract;

namespace DownloadsManager.UserControls
{
    /// <summary>
    /// Interaction logic for DownloadViewer.xaml
    /// </summary>
    public partial class DownloadViewer : UserControl, IControl
    {
        /// <summary>
        /// ctor
        /// </summary>
        public DownloadViewer(IDownloadViewerVM model)
        {
            InitializeComponent();
            this.DataContext = model;
        }

        /// <summary>
        /// Model for download viewer control
        /// </summary>
        public DownloadViewerVM Model
        {
            get
            {
                return this.DataContext as DownloadViewerVM;
            }
        }

    }
}
