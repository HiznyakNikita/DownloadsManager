using DownloadsManager.Core.Concrete.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DownloadsManager.Core.Concrete.Helpers
{
    public static class FileTypeIdentifier
    {
        private static string videoPattern = ".*\\.(wmv|mov|avi|divx|mpeg|mpg|m4p|avi)";
        private static string picturePattern = ".*\\.(jpg|jpeg|jfif|exitf|tiff|gif|bmp|png|ppm|pgm|pbm|pnm|webp)";
        private static string documentPattern = ".*\\.(pdf|doc|docx|djvu|txt|dot|docm|dotx|dotm|docb|xls|xlt|xlm|xlsx"
            + "|xlsm|xltx|xltm|xlsb|xla|xlam|xll|xlw|ppt|pot|pps|pptx|pptm|potx|potm|ppam|ppsx|ppsm|sldx|sldm)";
        private static string musicPattern = ".*\\.(mp3|aac|wav|mpa|ra|cda|flac|m4a|mid|mka|wv|tta|ac3|wma|mpc|ape|ofe|ogg|mp2|dts)";
        private static string applicationPattern = ".*\\.(exe|msi)";

        public static FileType IdentifyType(string name)
        {
            Regex regexVideo = new Regex(videoPattern);
            Regex regexPicture = new Regex(picturePattern);
            Regex regexDocument = new Regex(documentPattern);
            Regex regexMusic = new Regex(musicPattern);
            Regex regexApplication = new Regex(applicationPattern);

            if (regexVideo.IsMatch(name))
                return FileType.Video;
            if (regexPicture.IsMatch(name))
                return FileType.Picture;
            if (regexMusic.IsMatch(name))
                return FileType.Music;
            if (regexDocument.IsMatch(name))
                return FileType.Document;
            if (regexApplication.IsMatch(name))
                return FileType.Application;
            return FileType.Other;
        }

    }
}
