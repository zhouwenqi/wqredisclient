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
        /// <summary>
        /// add connection
        /// </summary>
        /// <param name="connection"></param>
        /// <returns>Server</returns>
        public static RedisServer addConnection(RedisConnection connection)
        {
            RedisClient redisClient = new RedisClient(connection.Host, Convert.ToInt32(connection.Port)); 
            RedisServer redisServer = new RedisServer { Connection = connection,Uid=Guid.NewGuid().ToString()};            
            redisServer.RedisClient = redisClient;
            redisServer.RedisClient.Encoding = Encoding.UTF8;           
            App.redisServers.Add(redisServer);
            return redisServer;
        }
        /// <summary>
        /// update serdis-server info
        /// </summary>
        /// <param name="redisServer"></param>
        /// <param name="connection"></param>
        public static void updateConnection(RedisServer redisServer, RedisConnection connection)
        {
            redisServer.Connection = connection;
            redisServer.RedisClient = new RedisClient(connection.Host, Convert.ToInt32(connection.Port));            
            redisServer.RedisClient.Encoding = Encoding.UTF8;
        }
        /// <summary>
        /// get redis-connection by hashCode
        /// </summary>
        /// <param name="hashCode"></param>
        /// <returns></returns>
        public static RedisConnection getRedisConnection(int hashCode)
        {
            foreach (RedisConnection conn in App.config.RedisConnections)
            {
                if (hashCode.Equals(conn.GetHashCode()))
                {
                    return conn;
                }
            }
            return null;
        }
        /// <summary>
        /// delete redis-server
        /// </summary>
        /// <param name="hashCode"></param>
        public static void deleteRedisConnection(int hashCode)
        {
            for(int i=App.config.RedisConnections.Count - 1; i >= 0; i--)
            {
                if (hashCode.Equals(App.config.RedisConnections[i].GetHashCode()))
                {
                    RedisConnection conn = App.config.RedisConnections[i];
                    App.config.RedisConnections.Remove(conn);
                }
            }
        }

        /// <summary>
        /// get redis-server by client
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
        /// find redis-server by uid
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static RedisServer getRedisServer(string uid)
        {
            foreach (RedisServer redisServer in App.redisServers)
            {
                if (redisServer.Uid.Equals(uid))
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
        public static RedisDatabase getDatabase(RedisServer redisServer, string dbName)
        {
            foreach(RedisDatabase database in redisServer.Databases)
            {
                if (dbName.Equals(database.Name))
                {
                    return database;
                }
            }
            return null;
        }
    }
}
