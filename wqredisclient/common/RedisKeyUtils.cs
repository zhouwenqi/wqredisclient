using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using wqredisclient.entity;
using wqredisclient.common;
using System.Collections.Specialized;

namespace wqredisclient.common
{
    public static class RedisKeyUtils
    {
        public static char[] sp = new char[] { ':' };
        public static ObservableCollection<RedisKey> getRedisKeys(string[] keys)
        {
            ObservableCollection<RedisKey> redisKeys = new ObservableCollection<RedisKey>();

            Dictionary<string, string> setKeys = new Dictionary<string, string>();
            if(keys.Length > 0)
            {
                foreach(string key in keys)
                {
                    if (string.IsNullOrEmpty(key))
                    {
                        break;
                    }
                    string[] str = key.Split(sp, 2);
                    string value = str.Length > 1 ? str[1] : null;                    
                    setKeys[str[0]] = value;
                }
            }
            foreach(string key in setKeys.Keys)
            {
                string value = setKeys[key];
                RedisKey redisKey = new RedisKey();                
                redisKey.Name = key;
                if (!string.IsNullOrEmpty(value))
                {
                    redisKey.NodeKey = setKeys[key];
                }else
                {
                    redisKey.Key = value;
                }              
                redisKeys.Add(redisKey);
            }
            return redisKeys;
        }
        public static ObservableCollection<RedisKey> getSplitStr(string[] keys)
        {
            Dictionary<string, HashSet<String>> dicKeys = new Dictionary<string, HashSet<String>>();
            ObservableCollection<RedisKey> redisKeys = new ObservableCollection<RedisKey>();
            if (keys.Length > 0)
            {
                foreach(string key in keys)
                {
                    if (string.IsNullOrEmpty(key))
                    {
                        break;
                    }
                    string[] str = key.Split(sp, 2,StringSplitOptions.None);
                    string k = str[0];
                    string v = str.Length > 1 ? str[1] : null;
                    if(!dicKeys.ContainsKey(str[0]))
                    {
                        dicKeys[k] = new HashSet<string>();
                    }
                    dicKeys[k].Add(v);
                }
                foreach(var item in dicKeys)
                {
                    RedisKey redisKey = new RedisKey();
                    redisKey.Name = item.Key;
                    if (item.Value != null && item.Value.Count > 0)
                    {
                        string[] str = new string[item.Value.Count];
                        item.Value.CopyTo(str);
                        redisKey.Keys = getSplitStr(str);
                    }
                    else
                    {
                        redisKey.Key = item.Key;
                    }
                    redisKeys.Add(redisKey);
                }
            }
            return redisKeys;
        }
        public static void  getSplitKey(RedisKey redisKey)
        {
            string[] keys = redisKey.NodeKey.Split(sp, 2);
            Dictionary<string, string> setKeys = new Dictionary<string, string>();
            if (keys.Length > 0)
            {
                foreach (string key in keys)
                {
                    string[] str = key.Split(sp, 2);
                    string value = str.Length > 1 ? str[1] : null;
                    setKeys[str[0]] = value;
                }
                foreach(var item in setKeys)
                {
                    RedisKey iKey = new RedisKey();
                    iKey.Name = item.Key;

                    foreach(string key in keys)
                    {

                    }
                }
            }
        }
    }
}
