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
        private string login;
        private string password;
        private IProtocolProvider provider;

        #endregion

        #region Constructor
        public ResourceInfo()
        {
        }
        #endregion

        #region Properties

        public string URL
        {
            get { return url; }
            set { url = value; }
        }

        public bool Authenticate
        {
            get { return authenticate; }
            set { authenticate = value; }
        }

        public string Login
        {
            get { return login; }
            set { login = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        #endregion

        #region Methods

        public static ResourceInfo FromURL(string url)
        {
            ResourceInfo ri = new ResourceInfo();
            ri.URL = url;
            return ri;
        }

        public static ResourceInfo[] FromURLArray(string[] urls)
        {
            List<ResourceInfo> result = new List<ResourceInfo>();

            for (int i = 0; i < urls.Length; i++)
            {
                ////TODO check is it url by regex
                result.Add(ResourceInfo.FromURL(urls[i]));
            }

            return result.ToArray();
        }

        public static ResourceInfo FromURL(
            string url,
            bool authenticate,
            string login,
            string password)
        {
            ResourceInfo ri = new ResourceInfo();
            ri.URL = url;
            ri.Authenticate = authenticate;
            ri.Login = login;
            ri.Password = password;
            return ri;
        } 

        /// <summary>
        /// Gets protocol provider for resource
        /// </summary>
        /// <returns>IProtocolProvider object for resource</returns>
        public IProtocolProvider GetProtocolProvider()
        {
            return BindProtocolProviderInstance();
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
            return this.URL;
        }

        #endregion
    }
}
