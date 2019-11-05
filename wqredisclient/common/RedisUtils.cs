using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wqredisclient.entity;
using CSRedis;

namespace wqredisclient.common
{
    public class RedisUtils
    {
        public static void addConnection(RedisConnection connection)
        {
            RedisClient redisClient = new RedisClient(connection.Host, Convert.ToInt32(connection.Port));
            
                       
            RedisServer redisServer = new RedisServer { Connection = connection };            
            redisServer.RedisClient = redisClient;
            App.redisServers.Add(redisServer);
        }
        /// <summary>
        /// get redis object
        /// </summary>
        /// <param name="redisClient"></param>
        /// <returns></returns>
        public static RedisServer getRedisServer(RedisClient redisClient)
        {
            foreach(RedisServer redisServer in App.redisServers)
            {
                if (redisServer.RedisClient.GetHashCode() == redisClient.GetHashCode())
                {
                    return redisServer;
                }
            }
            return null;
        }
        /// <summary>
        /// database count
        /// </summary>
        /// <param name="redisClient"></param>
        /// <returns></returns>
        public static int getDatabasesCount(RedisClient redisClient)
        {
            Tuple<string, string>[] config = redisClient.ConfigGet("databases");
            if(config == null || config.Length < 1)
            {
                return 0;
            }
            return Convert.ToInt16(config[0].Item2);
        }
    }
}
