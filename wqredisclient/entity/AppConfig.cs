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
        /// <summary>
        /// redis connections
        /// </summary>
        public List<RedisConnection> RedisConnections { get; set; }
    }
}
