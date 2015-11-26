using DownloadsManager.Core.Abstract;
using DownloadsManager.Core.Concrete;
using DownloadsManager.Core.Protocol;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Tests
{
    /// <summary>
    /// Test for ResourceInfo class
    /// </summary>
    [TestClass]
    public class ResourceInfoTest
    {
        [TestMethod]
        public void ResourceInfoFromUrlTest()
        {
            ResourceInfo info = ResourceInfo.FromUrl("http:\\test.org");
            Assert.AreEqual("http:\\test.org", info.Url);
        }

        [TestMethod]
        public void BindProtocolProviderTest()
        {
            ResourceInfo info = ResourceInfo.FromUrl("http:\\test.org");
            IProtocolProvider res = info.BindProtocolProviderInstance();
            Assert.AreEqual(typeof(HttpProtocolProvider), res.GetType());
        }

        [TestMethod]
        public void BindProtocolProviderProxyTest()
        {
            ResourceInfo info = ResourceInfo.FromUrl("http:\\test.org");
            IProtocolProvider res = 
                info.BindProtocolProviderProxy(new Core.Concrete.Helpers.SpeedLimitHelper(new Downloader()));
            Assert.AreEqual(typeof(HttpProtocolProviderSpeedLimitProxy), res.GetType());
        }
    }
}
