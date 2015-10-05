using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Core.Abstract
{
    public interface IFilter<T>
    {
        /// <summary>
        /// Method for check if object match with filter or not
        /// </summary>
        /// <param name="obj">object to match</param>
        /// <returns>boolean result of match</returns>
        bool IsSuitable(T obj);
    }
}
