using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace DownloadsManager.Core.Concrete.Helpers
{
    /// <summary>
    /// Helper class for location file at disk
    /// </summary>
    public static class LocalFilesHelper
    {
        /// <summary>
        /// method for locate file on disk
        /// </summary>
        /// <param name="localFile">local file info</param>
        /// <param name="fileName">name of file</param>
        /// <param name="fileSize">size of file</param>
        /// <returns>local file</returns>
        public static string LocateLocalFile(string localFile, string fileName, long fileSize)
        {
            string newFileName = localFile;

            if (!Directory.Exists(localFile))
            {
                Directory.CreateDirectory(localFile);
            }

            if (new FileInfo(localFile).Exists)
            {
                int currentVersion = 0;
                do
                {
                    newFileName = localFile + currentVersion.ToString(CultureInfo.CurrentCulture);
                }
                while (new FileInfo(newFileName).Exists);
            }

            using (FileStream fs = new FileStream(localFile + "\\" + fileName, FileMode.Create, FileAccess.Write))
            {
                fs.SetLength(Math.Max(fileSize, 0));
            }

            return newFileName;
        }
    }
}
