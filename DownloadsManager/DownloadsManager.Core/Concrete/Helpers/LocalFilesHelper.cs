using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace DownloadsManager.Core.Concrete.Helpers
{
    public static class LocalFilesHelper
    {
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
