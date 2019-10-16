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
        /// <summary>
        /// redis server info
        /// </summary>
        public RedisServer Server { set; get; }
        /// <summary>
        /// ssl
        /// </summary>
        public RedisSSL Ssl { set; get; }
        /// <summary>
        /// use protocol SSL
        /// </summary>
        public bool IsUseSSL { set; get; }
        /// <summary>
        /// ssh
        /// </summary>
        public RedisSSH Ssh { set; get; }
        /// <summary>
        /// use protocol SSH
        /// </summary>
        public bool IsUseSSH { set; get; }

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
