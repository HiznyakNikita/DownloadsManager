using DownloadsManager.Core.Abstract;
using DownloadsManager.Core.Concrete;
using DownloadsManager.Core.Concrete.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Core.Protocol
{
    /// <summary>
    /// Proxy for protocol provider need to enable speed limits
    /// </summary>
    [Serializable]
    public class HttpProtocolProviderSpeedLimitProxy : IProtocolProvider
    {
        private IProtocolProvider proxy;
        private SpeedLimitHelper speedLimit;

        public HttpProtocolProviderSpeedLimitProxy(IProtocolProvider proxy, SpeedLimitHelper speedLimit)
        {
            this.proxy = proxy;
            this.speedLimit = speedLimit;
        }

        #region IProtocolProvider Members

        public System.IO.Stream CreateResponseStream(ResourceInfo resourceInfo, int startRangePosition, int endRangePosition)
        {
            return new LimitedSpeedProxyStream(proxy.CreateResponseStream(resourceInfo, startRangePosition, endRangePosition), speedLimit);
        }

        public RemoteFileInfo GetFileInfo(ResourceInfo resourceInfo, out System.IO.Stream stream)
        {
            RemoteFileInfo result = proxy.GetFileInfo(resourceInfo, out stream);

            if (stream != null)
            {
                stream = new LimitedSpeedProxyStream(stream, speedLimit);
            }

            return result;
        }

        #endregion
    }
}
