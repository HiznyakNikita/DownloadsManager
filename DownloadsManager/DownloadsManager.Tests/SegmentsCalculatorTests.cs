using DownloadsManager.Core.Abstract;
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
    public class SegmentsCalculatorTests
    {
        private static List<CalculatedFileSegment> GetSegmentsFromCalculator()
        {
            FileSegmentCalculator calculator = new FileSegmentSizeCalculatorWithResidueHelper();
            RemoteFileInfo remoteFileInfo = new RemoteFileInfo();
            remoteFileInfo.AcceptRanges = true;
            remoteFileInfo.FileSize = 4724748;
            int segmentCount = 5;
            List<CalculatedFileSegment> segments = calculator.GetSegments(segmentCount,remoteFileInfo);
            return segments;
        }

        [TestMethod]
        public void TestSegmentSizeCalculation()
        {
            List<CalculatedFileSegment> segments = GetSegmentsFromCalculator();
            Assert.AreEqual(segments[0].SegmentEndPosition, 944949);
        }

        //Test for calculation size of last segment if we have some residue bytes after dividing
        [TestMethod]
        public void TestLastSegmentSizeCalculation()
        {
            List<CalculatedFileSegment> segments = GetSegmentsFromCalculator();
            Assert.AreEqual(segments[4].SegmentEndPosition - segments[4].SegmentStartPosition, 944952);
        }

        //Test recalculation of segment size if segment size less than minimum in settings
        [TestMethod]
        public void TestMinimumSegmentSizeCalculation()
        {
            FileSegmentCalculator calculator = new FileSegmentSizeCalculatorWithResidueHelper();
            RemoteFileInfo remoteFileInfo = new RemoteFileInfo();
            remoteFileInfo.AcceptRanges = true;
            remoteFileInfo.FileSize = 1773484;
            int segmentCount = 6;
            List<CalculatedFileSegment> segments = calculator.GetSegments(segmentCount, remoteFileInfo);
            Assert.AreEqual(segments[0].SegmentEndPosition, 354696);
        }
    }
}
