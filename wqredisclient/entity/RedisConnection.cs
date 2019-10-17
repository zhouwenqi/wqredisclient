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
    public class RedisConnection
    {
        private bool isUseSSL = false;
        private bool isUseSSH = false;
        private int connectionTimeOut = 60000;
        private int executionTimeOut = 60000;
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Host
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// Port
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// Auth
        /// </summary>
        public string Auth { get; set; }
        /// <summary>
        /// Connection Timeout
        /// </summary>
        public int ConnectionTimeOut
        {
            get
            {
                return connectionTimeOut;
            }
            set
            {
                connectionTimeOut = value;
            }
        }
        /// <summary>
        /// Execution Timeout
        /// </summary>
        public int ExecutionTimeOut
        {
            get
            {
                return executionTimeOut;
            }
            set
            {
                executionTimeOut = value;
            }
        }        
        /// <summary>
        /// ssl
        /// </summary>
        public RedisSSL Ssl { set; get; }
        /// <summary>
        /// use protocol SSL
        /// </summary>
        public bool IsUseSSL {
            get
            {
                return isUseSSL;
            }
            set
            {
                isUseSSL = value;
            }
        }
        /// <summary>
        /// ssh
        /// </summary>
        public RedisSSH Ssh { set; get; }
        /// <summary>
        /// use protocol SSH
        /// </summary>
        public bool IsUseSSH {
            get
            {
                return isUseSSH;
            }
            set
            {
                isUseSSH = value;
            }
        }
    }
    public class RedisSSL
    {
        /// <summary>
        /// public key
        /// </summary>
        public string PublicKey { get; set; }
        /// <summary>
        /// private key
        /// </summary>
        public string PrivateKey { get; set; }
        /// <summary>
        /// authority
        /// </summary>
        public string Authority { get; set; }
    }
    public class RedisSSH
    {
        /// <summary>
        /// remoto host
        /// </summary>
        public string RemotoHost { get; set; }
        /// <summary>
        /// remoto port
        /// </summary>
        public int RemotoPort { get; set; }
        /// <summary>
        /// username
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// password
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// private key
        /// </summary>
        public string PrivateKey { get; set; }
    }

}
