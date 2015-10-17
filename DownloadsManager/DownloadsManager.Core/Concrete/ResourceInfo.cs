using DownloadsManager.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DownloadsManager.Core.Concrete
{
    public class ResourceInfo
    {
        #region Fields
        
        private string url;
        private bool authenticate;
        private string logOn;
        private string password;
        private IProtocolProvider provider;

        #endregion

        #region Constructor
        public ResourceInfo()
        {
        }
        #endregion

        #region Properties

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public bool Authenticate
        {
            get { return authenticate; }
            set { authenticate = value; }
        }

        public string LogOn
        {
            get { return logOn; }
            set { logOn = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        #endregion

        #region Methods

        public static ResourceInfo FromUrl(string url)
        {
            ResourceInfo ri = new ResourceInfo();
            ri.Url = url;
            return ri;
        }

        public static ResourceInfo[] FromUrlArray(string[] urls)
        {
            List<ResourceInfo> result = new List<ResourceInfo>();
            if(urls != null)
            for (int i = 0; i < urls.Length; i++)
            {
                ////TODO check is it url by regex
                result.Add(ResourceInfo.FromUrl(urls[i]));
            }

            return result.ToArray();
        }

        public static ResourceInfo FromUrl(
            string url,
            bool authenticate,
            string logOn,
            string password)
        {
            ResourceInfo ri = new ResourceInfo();
            ri.Url = url;
            ri.Authenticate = authenticate;
            ri.LogOn = logOn;
            ri.Password = password;
            return ri;
        } 

        /// <summary>
        /// Gets protocol provider for resource
        /// </summary>
        /// <returns>IProtocolProvider object for resource</returns>
        public IProtocolProvider ProtocolProvider
        {
            get
            {
                return BindProtocolProviderInstance();
            }
        }

        /// <summary>
        /// bind protocol provider for resource
        /// </summary>
        /// <returns>IProtocolProvider object for resource</returns>
        public IProtocolProvider BindProtocolProviderInstance()
        {
            if (provider == null)
            {
                provider = new HttpProtocolProvider();
            }

            return provider;
        }

        /// <summary>
        /// ovverride to string
        /// </summary>
        /// <returns>URL of resource</returns>
        public override string ToString()
        {
            return this.Url;
        }

        #endregion
    }
}
