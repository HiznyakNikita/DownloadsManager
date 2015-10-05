using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Core.Abstract
{
    /// <summary>
    /// Interface for implementing filter operations in downloads list
    /// </summary>
    /// <typeparam name="T">filtered type</typeparam>
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
