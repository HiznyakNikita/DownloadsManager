using DownloadsManager.Helpers.Concrete;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Tests
{
    [TestClass]
    public class UiHelpersTest
    {
        [TestMethod]
        public void BytesToKBConverterTest()
        {
            Assert.AreEqual(Math.Round((Double)2876 / 1024, 4), Converters.BytesToKBConverter(2876));
        }

        [TestMethod]
        public void BytesToMBConverterTest()
        {
            Assert.AreEqual(Math.Round((Double)2876 / (1024 * 1024), 4), Converters.BytesToMBConverter(2876));
        }

        [TestMethod]
        public void BytesToGBConverterTest()
        {
            Assert.AreEqual(Math.Round((Double)2876 / (1024 * 1024 * 1024), 4), Converters.BytesToGBConverter(2876));
        }
    }
}
