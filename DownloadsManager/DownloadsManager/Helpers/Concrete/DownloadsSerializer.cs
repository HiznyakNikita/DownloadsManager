using DownloadsManager.Core.Concrete;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DownloadsManager.Helpers.Concrete
{
    /// <summary>
    /// Serializer for saving downloads
    /// </summary>
    public static class DownloadsSerializer
    {
        public static void Serialize(List<Downloader> downloads)
        {
            try
            {
                using (var fs = File.Create("downloads.bin"))
                {
                    new BinaryFormatter().Serialize(fs, downloads);
                    fs.Flush();
                }
            }
            catch (SerializationException)
            {

            }
            catch (Exception)
            {

            }
        }

        public static List<Downloader> Deserialize()
        {
            try
            {
                using (var fs = File.Open("downloads.bin", FileMode.Open))
                {
                    return (List<Downloader>)new BinaryFormatter().Deserialize(fs);
                }
            }
            catch (FileNotFoundException)
            {
                using (var fs = File.Create("downloads.bin"))
                {
                    return new List<Downloader>();
                }
            }
            catch (SerializationException)
            {
                return new List<Downloader>();
            }
            catch (Exception)
            {
                return new List<Downloader>();
            }
        }
    }
}
