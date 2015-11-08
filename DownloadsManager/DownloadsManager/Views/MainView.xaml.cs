﻿using DownloadsManager.Core.Concrete;
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
        /// <param name="model"> view model</param>
        public MainView(IMainWindowVM model)
        {
            InitializeComponent();
            this.DataContext = _model = model;
            timer.Interval = TimeSpan.FromSeconds(0.5);
            timer.Tick += Timer_Tick;
            timer.Start();
            (model as MainWindowVM).DownloadEndedViewModel += DownloaderManager_DownloadEnded;
        }

        private void DownloaderManager_DownloadEnded(object sender, EventArgs e)
        {
            TrayIcon.ShowBalloonTip(5, "Download finished!", "Download: " + (e as DownloadEndedEventArgs).DownloadName + " finished!", System.Windows.Forms.ToolTipIcon.Info);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.itemsControlDownloads.ItemsSource = null;
            this.itemsControlDownloads.ItemsSource = _model.GetDownloaders().Values;
        }

        private void BtnHideWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void BtnChooseHistoryPeriod_Click(object sender, RoutedEventArgs e)
        {
            if (datePickerFrom.SelectedDate != null && datePickerTo.SelectedDate != null)
            {
                (_model as MainWindowVM).AddHistoryParams((DateTime)datePickerFrom.SelectedDate, (DateTime)datePickerTo.SelectedDate);
            }
            else
            {
                MessageBox.Show("Wrong seleted dates");
            }
        }

        #region Tray
        /// <summary>
        /// override method for init
        /// </summary>
        /// <param name="e"></param>

        protected override void OnSourceInitialized(EventArgs e)
        {
            // base for return in previous state
            base.OnSourceInitialized(e);
            CreateTrayIcon();
        }
        private System.Windows.Forms.NotifyIcon TrayIcon = null;
        private System.Windows.Controls.ContextMenu TrayMenu = null;

        private bool CreateTrayIcon()
        {
            bool result = false;
            if (TrayIcon == null)
            {
                TrayIcon = new System.Windows.Forms.NotifyIcon();
                TrayIcon.Icon = DownloadsManager.Properties.Resources.icon;
                TrayIcon.Text = DownloadsManager.Properties.Resources.Name;
                TrayMenu = Resources["TrayMenu"] as System.Windows.Controls.ContextMenu;
                TrayIcon.Click += delegate(object sender, EventArgs e)
                {
                    if ((e as System.Windows.Forms.MouseEventArgs).Button == System.Windows.Forms.MouseButtons.Left)
                    {

                        ShowHideMainWindow(sender, null);
                    }
                    else
                    {

                        TrayMenu.IsOpen = true;
                        Activate();
                    }
                };
                result = true;
            }
            else
            {
                result = true;
            }
            TrayIcon.Visible = true;
            return result;
        }

        private void ShowHideMainWindow(object sender, RoutedEventArgs e)
        {
            TrayMenu.IsOpen = false;
            if (IsVisible)
            {
                Hide();

                (TrayMenu.Items[0] as System.Windows.Controls.MenuItem).Header = "Show";
            }
            else
            {
                Show();

                (TrayMenu.Items[0] as System.Windows.Controls.MenuItem).Header = "Hide";
                WindowState = CurrentWindowState;
                Activate();
            }
        }
        
        private WindowState fCurrentWindowState = WindowState.Maximized;
        
        /// <summary>
        /// Temporary window state
        /// </summary>
        public WindowState CurrentWindowState
        {
            get { return fCurrentWindowState; }
            set { fCurrentWindowState = value; }
        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);
            if (this.WindowState == System.Windows.WindowState.Minimized)
            {

                Hide();

                (TrayMenu.Items[0] as System.Windows.Controls.MenuItem).Header = "Show";
            }
            else
            {
                CurrentWindowState = WindowState;
            }
        }
        private bool fCanClose = false;
        public bool CanClose
        {
            get { return fCanClose; }
            set { fCanClose = value; }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            if (!CanClose)
            {

            }
        }
        #endregion

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TrayIcon.Icon = null;
            Application.Current.Shutdown();
        }
    }
}
