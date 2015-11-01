using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DownloadsManager.Core.Concrete.Helpers
{
    public class SpeedLimitHelper
    {
        private Downloader _downloader;

        private const int upBalance = 50;
        private const int downBalance = -80;

        private double currentWait;

        #region Properties

        public double CurrentMaxRate
        {
            get { return _downloader.MaxRate; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method for waiting in stream to regulate downloading speed
        /// </summary>
        public void WaitFor()
        {
            double tempRate = _downloader.Rate;

            if (tempRate > CurrentMaxRate*100)
            {
                currentWait += upBalance;
            }
            else
            {
                currentWait = Math.Max(currentWait + downBalance, 0);
            }

            Thread.Sleep(TimeSpan.FromMilliseconds(currentWait));

            Debug.WriteLine("rate = " + tempRate);
            Debug.WriteLine("maxLimit = " + CurrentMaxRate);
            Debug.WriteLine("currentWait = " + currentWait);
        }

        #endregion

        #region Constructor

        public SpeedLimitHelper(Downloader downloader)
        {
            _downloader = downloader;
        }

        #endregion
    }
}
