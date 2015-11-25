using Autofac;
using DownloadsManager.Helpers.Autofac;
using DownloadsManager.UserControls;
using DownloadsManager.UserControls.Abstract;
using DownloadsManager.ViewModels;
using DownloadsManager.ViewModels.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DownloadsManager.Helpers.Concrete
{
    /// <summary>
    /// COntrol creator realization
    /// </summary>
    public class ControlCreator : IControlCreator
    {
        //Create control and attach view model
        public IControl CreateControl(IMainVM model)
        {
            if (model is DownloadViewerVM)
                using (var container = AutofacHelper.Container)
                {
                    return new DownloadViewer(model as IDownloadViewerVM);
                }

            if (model is FindFolderControlVM)
                using (var container = AutofacHelper.Container)
                {
                    return new FindFolderControl(model as IFindFolderControlVM);
                }

            return null;
        }
    }
}
