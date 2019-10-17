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
            RedisClient redisClient = new RedisClient(connection.Host, connection.Port);
            RedisServer redisServer = new RedisServer { Connection = connection };            
            redisServer.RedisClient = redisClient;
            App.redisServers.Add(redisServer);
        }
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
    }
}
