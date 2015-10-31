using DownloadsManager.Core.Concrete;
using DownloadsManager.Core.Concrete.DownloadStates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DownloadsManager.Tests
{
    [TestClass]
    public class DownloaderTest
    {
        //[TestMethod]
        //public void DownloadingFileExistTest()
        //{
        //    ResourceInfo resourceInfo = new ResourceInfo();
        //    resourceInfo.Url = "http://victoriafallsriverlodge.com/wp-content/uploads/2013/09/Lodge-from-walkway.jpg";
        //    Downloader download = new Downloader(
        //        resourceInfo, null, Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.jpg");
        //    DownloaderManager.Instance.Add(download, true);
        //    Thread.Sleep(30000);
        //    Assert.AreEqual(true, File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\test.jpg"));
        //    File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\test.jpg");
        //}

        //[TestMethod]
        //public void DownloadingFileEndedTest()
        //{
        //    ResourceInfo resourceInfo = new ResourceInfo();
        //    resourceInfo.Url = "http://victoriafallsriverlodge.com/wp-content/uploads/2013/09/Lodge-from-walkway.jpg";
        //    Downloader download = new Downloader(
        //        resourceInfo, null, Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.jpg");
        //    DownloaderManager.Instance.Add(download, true);
        //    Thread.Sleep(30000);
        //    Assert.AreEqual(DownloadState.Ended, download.State.State);
        //    File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\test.jpg");
        //}

        //[TestMethod]
        //public void DownloadSegmentationTest()
        //{
        //    ResourceInfo resourceInfo = new ResourceInfo();
        //    resourceInfo.Url = "http://victoriafallsriverlodge.com/wp-content/uploads/2013/09/Lodge-from-walkway.jpg";
        //    Downloader download = new Downloader(
        //        resourceInfo, null, Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.jpg");
        //    DownloaderManager.Instance.Add(download, true);
        //    Thread.Sleep(20000);
        //    Assert.AreEqual(5, download.FileSegments.Count);
        //    File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\test.jpg");
        //}

        //[TestMethod]
        //public void PausedResumedDownloadTest()
        //{
        //    ResourceInfo resourceInfo = new ResourceInfo();
        //    resourceInfo.Url = "http://victoriafallsriverlodge.com/wp-content/uploads/2013/09/Lodge-from-walkway.jpg";
        //    Downloader download = new Downloader(
        //        resourceInfo, null, Environment.GetFolderPath(Environment.SpecialFolder.Desktop), 4, "test.jpg");
        //    DownloaderManager.Instance.Add(download, true);
        //    Thread.Sleep(15000);
        //    download.Pause();
        //    Thread.Sleep(2000);
        //    download.Start();
        //    Thread.Sleep(30000);
        //    Assert.AreEqual(typeof(DownloadEndedState), download.State.GetType());
        //    File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\test.jpg");
        //}

        ////Test for one segment download, we must skip segment and start download from 0 if we paused
        //[TestMethod]
        //public void SkipOneSegmentIfPausedTest()
        //{
        //    ResourceInfo resourceInfo = new ResourceInfo();
        //    resourceInfo.Url = "https://www.dropbox.com/s/8lbatjtdues8mxh/ManagerVk_30_06_15.rar?dl=1";
        //    Downloader download = new Downloader(
        //        resourceInfo, null, Environment.GetFolderPath(Environment.SpecialFolder.Desktop), 1, "test.rar");
        //    DownloaderManager.Instance.Add(download, true);
        //    Thread.Sleep(20000);
        //    download.Pause();
        //    Thread.Sleep(2000);
        //    Assert.AreEqual(0, download.FileSegments[0].Progress);
        //    Assert.AreEqual(0, download.FileSegments[0].StartPosition);
        //    File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\test.rar");
        //}

        ////Test for one segment download, we must start from second(or more) segment if we paused
        //[TestMethod]
        //public void StartFromNextSegmentIfPausedTest()
        //{
        //    ResourceInfo resourceInfo = new ResourceInfo();
        //    resourceInfo.Url = "http://victoriafallsriverlodge.com/wp-content/uploads/2013/09/Lodge-from-walkway.jpg";
        //    Downloader download = new Downloader(
        //        resourceInfo, null, Environment.GetFolderPath(Environment.SpecialFolder.Desktop), 4, "test.jpg");
        //    DownloaderManager.Instance.Add(download, true);
        //    Thread.Sleep(10000);
        //    download.Pause();
        //    Thread.Sleep(2000);
        //    Assert.AreEqual(100, download.FileSegments[0].Progress);
        //    File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\test.jpg");
        //}
    }
}
