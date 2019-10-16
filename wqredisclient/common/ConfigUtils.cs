using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.IO;
using wqredisclient.entity;
using Newtonsoft.Json;

namespace wqredisclient.common
{
    public static class ConfigUtils
    {
        public static string configPath = App.rooPath + "/config.json";
        public static void loadAppConfig()
        {
            string configPath = App.rooPath + "/config.json";
            if (!File.Exists(configPath))
            {
                initAppConfig();
            }
            StreamReader streamReader = new StreamReader(configPath, Encoding.UTF8);
            String line = null;
            StringBuilder sb = new StringBuilder();
            while ((line = streamReader.ReadLine()) != null)
            {
                sb.Append(line);
            }
            App.config = JsonConvert.DeserializeObject<AppConfig>(sb.ToString());
        }
        public static void initAppConfig()
        {
            AppConfig appConfig = new AppConfig();
            appConfig.RedisConnections = new List<RedisConnection>();
            saveConfig(appConfig);
        }
        public static void saveConfig(AppConfig appConfig)
        {
            appConfig.RedisConnections.ForEach((item) => {
                item.Server.Databases.Clear();
            });
            String jsonData = JsonConvert.SerializeObject(appConfig, App.jsonSettings);
            using (StreamWriter sw = new StreamWriter(configPath))
            {
                sw.Write(jsonData);
            }
        }
        public static void saveConfig()
        {
            saveConfig(App.config);
        }
    }
}
