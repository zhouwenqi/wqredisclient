using System;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using wqredisclient.entity;

namespace wqredisclient
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        // json setting
        public static JsonSerializerSettings jsonSettings;
        // app path
        public static string rooPath;
        // app config
        public static AppConfig config;

        private log4net.ILog log = log4net.LogManager.GetLogger("App.xaml.cs");
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            base.OnStartup(e);
            rooPath = AppDomain.CurrentDomain.BaseDirectory;
            jsonSettings = new JsonSerializerSettings();
            jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

        }
    }
}
