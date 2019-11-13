using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using wqredisclient.entity;
using wqredisclient.common;
using System.Collections.Specialized;
using System.Windows.Controls;
using System.Windows;

namespace wqredisclient.common
{
    public static class RedisKeyUtils
    {
        public static char[] sp = new char[] { ':' };
        public static ObservableCollection<RedisKey> getRedisKeys(string[] keys)
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
                        redisKey.Keys = getRedisKeys(str);
                    }                    
                    redisKeys.Add(redisKey);
                }
            }
            return redisKeys;
        }

        public static string getKeys(object sender,string key)
        {
            if(typeof(TreeViewItem) == sender.GetType())
            {
                TreeViewItem item = (TreeViewItem)sender;
                key = item.Header.ToString() + ":" + key;
                getKeys(item.Parent, key);
            }
            return key;
            
        }

        public static ObservableCollection<RedisKey> getSplitKeys(string[] keys,string prevKey)
        {
            Dictionary<string, HashSet<String>> dicKeys = new Dictionary<string, HashSet<String>>();
            ObservableCollection<RedisKey> redisKeys = new ObservableCollection<RedisKey>();
            if(keys == null)
            {
                return redisKeys;
            }
            if (keys.Length > 0)
            {
                foreach (string key in keys)
                {
                    if (string.IsNullOrEmpty(key))
                    {
                        continue;
                    }
                    
                    string[] str = key.Split(sp, 2, StringSplitOptions.None);
                    string k = str[0];
                    string v = str.Length > 1 ? str[1] : null;
                    if (!dicKeys.ContainsKey(str[0]))
                    {
                        dicKeys[k] = new HashSet<string>();
                    }
                    dicKeys[k].Add(v);
                }
                foreach (var item in dicKeys)
                {
                    RedisKey redisKey = new RedisKey();
                    redisKey.Name = item.Key;
                    if (string.IsNullOrEmpty(prevKey))
                    {
                        redisKey.Key = item.Key;
                    }else
                    {
                        redisKey.Key = prevKey + new String(sp) + item.Key;
                    }
                    if (item.Value != null && item.Value.Count > 0)
                    {
                        string[] str = new string[item.Value.Count];
                        item.Value.CopyTo(str);
                        redisKey.Keys = getSplitKeys(str,redisKey.Key);
                    }
                    redisKeys.Add(redisKey);
                }
            }
            return redisKeys;
        }
        public static string getKeyName(string key)
        {
            string[] keys = key.Split(sp);
            if(keys == null)
            {
                return key;
            }
            else
            {
                return keys[keys.Length - 1];
            }
        }

    }
}
