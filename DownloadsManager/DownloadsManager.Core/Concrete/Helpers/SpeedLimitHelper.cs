using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DownloadsManager.Core.Concrete.Helpers
{
    [Serializable]
    public class SpeedLimitHelper
    {
        private const int UpBalance = 50;
        private const int DownBalance = -80;

        private Downloader _downloader;

        private double currentWait;

        public SpeedLimitHelper(Downloader downloader)
        {
            _downloader = downloader;
        }

        #region Properties

        /// <summary>
        /// current maximum download rate
        /// </summary>
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

            if (tempRate > CurrentMaxRate * 100)
            {
                currentWait += UpBalance;
            }
            else
            {
                currentWait = Math.Max(currentWait + DownBalance, 0);
            }

            //wait for download in milliseonds
            Thread.Sleep(TimeSpan.FromMilliseconds(currentWait));

            Debug.WriteLine("rate = " + tempRate);
            Debug.WriteLine("maxLimit = " + CurrentMaxRate);
            Debug.WriteLine("currentWait = " + currentWait);
        }

        #endregion
    }
}
