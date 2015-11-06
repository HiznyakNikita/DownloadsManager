using DownloadsManager.Core.Concrete.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsManager.Core.Concrete
{
    public class LimitedSpeedProxyStream : Stream
    {
        private Stream proxy;
        private SpeedLimitHelper speedLimit;

        public LimitedSpeedProxyStream(Stream proxy, SpeedLimitHelper speedLimit)
        {
            this.speedLimit = speedLimit;
            this.proxy = proxy;
        }

        #region Stream

        public override bool CanRead
        {
            get { return proxy.CanRead; }
        }

        public override bool CanSeek
        {
            get { return proxy.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return proxy.CanWrite; }
        }

        public override long Length
        {
            get { return proxy.Length; }
        }

        public override long Position
        {
            get
            {
                return proxy.Position;
            }

            set
            {
                proxy.Position = value;
            }
        }

        public override void Flush()
        {
            proxy.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            speedLimit.WaitFor();

            return proxy.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return proxy.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            proxy.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            proxy.Write(buffer, offset, count);
        } 

        #endregion
    }
}
