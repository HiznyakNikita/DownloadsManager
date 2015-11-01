using DownloadsManager.UserControls.Abstract;
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

namespace DownloadsManager.UserControls
{
    /// <summary>
    /// Interaction logic for FindFolderControl.xaml
    /// </summary>
    public partial class FindFolderControl : UserControl, IControl
    {
        private IFindFolderControlVM _model;

        public FindFolderControl(IFindFolderControlVM model)
        {
            InitializeComponent();
            this.DataContext = _model = model;
        }

        private void btnChooseFolder_Click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = folderBrowserDialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    tbFolderPath.Text = (_model as FindFolderControlVM).FolderPath = folderBrowserDialog.SelectedPath;                  
                }
            }
        }
    }
}
