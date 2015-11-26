using DownloadsManager.Core.Concrete;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Tests
{
    [TestClass]
    public class FileSegmentTest
    {
        [TestMethod]
        public void TransferedBytesTest()
        {
            FileSegment segment = new FileSegment();
            segment.StartPosition = 200;
            segment.InitialStartPosition = 100;
            Assert.AreEqual(100, segment.TransferBytes);
        }

        [TestMethod]
        public void TotalToTransferTest()
        {
            FileSegment segment = new FileSegment();
            segment.EndPosition = 200;
            segment.InitialStartPosition = 100;
            Assert.AreEqual(100, segment.TotalToTransfer);
            segment.EndPosition = -1;
            Assert.AreEqual(0, segment.TotalToTransfer);
        }

        [TestMethod]
        public void MissingTransferTest()
        {
            FileSegment segment = new FileSegment();
            segment.StartPosition = 190;
            segment.EndPosition = 200;
            Assert.AreEqual(10, segment.MissingTransfer);
            segment.EndPosition = -1;
            Assert.AreEqual(0, segment.MissingTransfer);
        }
    }
}
