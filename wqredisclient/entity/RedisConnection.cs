using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wqredisclient.entity
{
    /// <summary>
    /// Redis Connection
    /// </summary>
    public class RedisConnection : BaseEntity
    {
        private string name;
        private string host;
        private string port = "6379";
        private string auth;
        private string keyPattern = "*";
        private string keySeparator = ":";
        private string connectionTimeOut = "60";
        private string executionTimeOut = "60";
        /// <summary>
        /// Name
        /// </summary>
        public string Name {
            get
            {
                return name;
            }
            set
            {
                UpdateProperty(ref name, value);
            }
        }
        /// <summary>
        /// Host
        /// </summary>
        public string Host
        {
            get
            {
                return host;
            }
            set
            {
                UpdateProperty(ref host, value);
            }
        }
        /// <summary>
        /// Port
        /// </summary>
        public string Port
        {
            get
            {
                return port;
            }
            set
            {
                UpdateProperty(ref port, value);
            }
        }
        /// <summary>
        /// Auth
        /// </summary>
        public string Auth
        {
            get
            {
                return auth;
            }
            set
            {
                UpdateProperty(ref auth, value);
            }
        }
        /// <summary>
        /// Connection Timeout
        /// </summary>
        public string ConnectionTimeOut
        {
            get
            {
                return connectionTimeOut;
            }
            set
            {
                UpdateProperty(ref connectionTimeOut, value);
            }
        }
        /// <summary>
        /// Execution Timeout
        /// </summary>
        public string ExecutionTimeOut
        {
            get
            {
                return executionTimeOut;
            }
            set
            {
                UpdateProperty(ref executionTimeOut, value);
            }
        }
        /// <summary>
        /// Global style pattern
        /// </summary>
        public string KeyPattern
        {
            get
            {
                return keyPattern;
            }
            set
            {
                UpdateProperty(ref keyPattern, value);
            }
        }
        /// <summary>
        /// Namespace peparator
        /// </summary>
        public string KeySeparator
        {
            get
            {
                return keySeparator;
            }
            set
            {
                UpdateProperty(ref keySeparator, value);
            }
        }
    }

}
