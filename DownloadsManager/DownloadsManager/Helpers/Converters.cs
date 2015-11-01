using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Helpers
{
    public static class Converters
    {
        public static double BytesToKBConverter(double bytes)
        {
            return Math.Round(bytes / 1024,4);
        }

        public static double BytesToMBConverter(double bytes)
        {
            return Math.Round(BytesToKBConverter(bytes) / 1024,4);
        }

        public static double BytesToGBConverter(double bytes)
        {
            return Math.Round(BytesToMBConverter(bytes) / 1024,4);
        }
    }
}
