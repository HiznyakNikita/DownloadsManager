using DownloadsManager.Core.Concrete;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DownloadsManager.Helpers
{
    public static class DownloadsSerializer
    {
        public static void Serialize(List<Downloader> downloads)
        {
            using (var fs = File.Create("test.bin"))
            {
                new BinaryFormatter().Serialize(fs, downloads);
                fs.Flush();
            }
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    using (FileStream file = new FileStream("d:\\file.txt", FileMode.Create, FileAccess.Write))
            //    {
            //        BinaryFormatter bin = new BinaryFormatter();
            //        bin.Serialize(ms, downloads);

            //        ms.WriteTo(file);
            //    }
            //}
            
        }

        public static List<Downloader> Deserialize()
        {

            using (var fs = File.Open("test.bin", FileMode.Open))
            {
                return (List<Downloader>)new BinaryFormatter().Deserialize(fs);
            }
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    using (FileStream file = new FileStream("d:\\file.txt", FileMode.Open, FileAccess.Read))
            //    {
            //        file.CopyTo(ms);
            //        BinaryFormatter bin = new BinaryFormatter();
            //        List<Downloader> downloads = (List<Downloader>)bin.Deserialize(file);
                    
            //        return downloads;
            //    }
            //}
        }
    }
}
