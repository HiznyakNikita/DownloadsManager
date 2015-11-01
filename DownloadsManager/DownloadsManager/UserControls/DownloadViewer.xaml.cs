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
using System.Globalization;
using DownloadsManager.Core.Concrete.Enums;

namespace DownloadsManager.UserControls
{

    public class FileTypeToTypeIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((FileType)value == FileType.Application)
                return "/Resources/icons/app_icon.png";
            if ((FileType)value == FileType.Document)
                return "/Resources/icons/doc_icon.png";
            if ((FileType)value == FileType.Music)
                return "/Resources/icons/music_icon.png";
            if ((FileType)value == FileType.Picture)
                return "/Resources/icons/picture_icon.png";
            if ((FileType)value == FileType.Video)
                return "/Resources/icons/video_icon.png";
            if ((FileType)value == FileType.Other)
                return "/Resources/icons/doc_icon.png";
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

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

        private void btnDownloadSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Convert.ToDouble(tbDownloadMaxRate.Text, new NumberFormatInfo());
            }
            catch(Exception)
            {
                MessageBox.Show("Wrong number in max rate!");
            }
        }

    }
}
