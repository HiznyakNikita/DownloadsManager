using DownloadsManager.Core.Concrete;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DownloadsManager.Core.Abstract
{
    public interface IProtocolProvider
    {
        /// <summary>
        /// Method for getting response stream for resource
        /// </summary>
        /// <param name="ri">information about remote resource</param>
        /// <param name="startRangePosition">start range position for segment download</param>
        /// <param name="endRangePosition">end position of segment download</param>
        /// <returns>stream</returns>
        Stream CreateResponseStream(ResourceInfo ri, int startRangePosition, int endRangePosition);

        /// <summary>
        /// Method for getting information about remote file on server
        /// </summary>
        /// <param name="ri">information about remote resource</param>
        /// <param name="stream">stream to write file</param>
        /// <returns>information about remote file on server</returns>
        RemoteFileInfo GetFileInfo(ResourceInfo ri, out Stream stream);
    }
}
