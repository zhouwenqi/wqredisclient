using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wqredisclient.entity
{
    /// <summary>
    /// Redis Server
    /// </summary>
    public class RedisServer
    {
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
        public int ConnectionTimeOut { get; set; }
        /// <summary>
        /// Execution Timeout
        /// </summary>
        public int ExecutionTimeOut { get; set; }
    }
}
