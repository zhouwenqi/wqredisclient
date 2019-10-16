using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wqredisclient.entity
{
    /// <summary>
    /// global config
    /// </summary>
    public class AppConfig
    {
        private List<RedisConnection> redisConnections = new List<RedisConnection>();
        /// <summary>
        /// redis connections
        /// </summary>
        public List<RedisConnection> RedisConnections { get { return this.redisConnections; }set { this.redisConnections = value; } }
    }
}
