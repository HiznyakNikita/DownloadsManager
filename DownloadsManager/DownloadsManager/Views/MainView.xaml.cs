using DownloadsManager.Core.Concrete;
using DownloadsManager.UserControls;
using DownloadsManager.ViewModels;
using DownloadsManager.ViewModels.Infrastructure;
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
using System.Windows.Threading;

[assembly: CLSCompliant(true)]

namespace DownloadsManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {

       private DispatcherTimer timer = new DispatcherTimer();
       private IMainWindowVM _model;

        /// <summary>
        /// MainWindow ctor
        /// </summary>
        public MainView(IMainWindowVM model)
        {
            InitializeComponent();
            this.DataContext = _model = model;
            timer.Interval = TimeSpan.FromSeconds(0.5);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.itemsControlDownloads.ItemsSource = null;
            this.itemsControlDownloads.ItemsSource = _model.GetDownloaders().Values;
        }

        private void BtnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItemAddDownload_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnHideWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
