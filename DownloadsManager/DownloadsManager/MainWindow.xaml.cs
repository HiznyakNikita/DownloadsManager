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
using System.Windows.Navigation;
using System.Windows.Shapes;

[assembly: CLSCompliant(true)]

namespace DownloadsManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// MainWindow ctor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowVM();
        }

        private void BtnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnTest_Click(object sender, RoutedEventArgs e)
        {
            Downloader d = new Downloader("a.txt", new RemoteFileInfo() { FileSize = 20 }, new ResourceInfo() { URL = "https://pp.vk.me/c627430/v627430201/162b1/1sI5qa1_xnw.jpg" }, DateTime.Now);
            d.Download("https://pp.vk.me/c627430/v627430201/162b1/1sI5qa1_xnw.jpg","D:\\Developer");
        }
    }
}
