using DownloadsManager.UserControls.Abstract;
using DownloadsManager.ViewModels.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DownloadsManager.Helpers
{
    /// <summary>
    /// Interfae for control creator
    /// </summary>
    public interface IControlCreator
    {
        IControl CreateControl(IMainVM model);
    }
}
