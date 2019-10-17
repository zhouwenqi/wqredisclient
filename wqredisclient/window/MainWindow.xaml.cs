using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using wqredisclient.entity;
using wqredisclient.common;
using System.Threading;
using System.Collections;

namespace wqredisclient.window
{
    /// <summary>
    /// main window
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            initServerList();
        }

        private void initServerList()
        {
            //List<RedisConnection> redisConnections = new List<RedisConnection>();
            //RedisConnection redisServer1 = new RedisConnection();            
            //redisServer1.Name = "192.168.1.51";
            //redisServer1.Host = "192.168.1.51";
            //redisServer1.Port = 6379;
            //RedisConnection redisServer2 = new RedisConnection();
            //redisServer2.Name = "192.168.3.220";
            //redisServer2.Host = "192.168.3.220";
            //redisServer2.Port = 6379;
            //RedisConnection redisServer3 = new RedisConnection();
            //redisServer3.Name = "localhost";
            //redisServer3.Host = "localhost";
            //redisServer3.Port = 6379;
            //RedisConnection redisServer4 = new RedisConnection();
            //redisServer4.Name = "jiangwei";
            //redisServer4.Host = "192.168.1.51";
            //redisServer4.Port = 6379;
            //redisConnections.Add(redisServer1);
            //redisConnections.Add(redisServer2);
            //redisConnections.Add(redisServer3);
            //redisConnections.Add(redisServer4);
            //App.config.RedisConnections = redisConnections;
            //ConfigUtils.saveConfig();

            App.config.RedisConnections.ForEach((connection) => {                
                RedisUtils.addConnection(connection);
            });           

            redisServerBox.Items.Clear();
            this.Dispatcher.Invoke(new Action(delegate
            {
                redisServerBox.ItemsSource = App.redisServers;
            }));

            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ResourceDictionary resource = new ResourceDictionary();
            resource.Source = new Uri("/language/zh_CN.xaml", UriKind.Relative);
            Console.WriteLine("size:" + Application.Current.Resources.MergedDictionaries.Count);
            Application.Current.Resources.MergedDictionaries[1] = resource;
        }

        private void redisServerBox_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            RedisServer redisServer = (RedisServer)e.NewValue;
            if(!redisServer.RedisClient.IsConnected)
            {
                redisServer.IsConnectioning = true;
                ThreadStart threadStart = new ThreadStart(() => {
                    redisConnecton(redisServer);
                });
                new Thread(threadStart).Start();
            }
        }

        private void redisConnecton(RedisServer redisServer)
        {
            CSRedis.RedisClient redisClient = redisServer.RedisClient;
            redisClient.Connected += RedisClient_Connected;            
            if (!redisClient.IsConnected)
            {
                try
                {
                    redisClient.Connect(redisServer.Connection.ConnectionTimeOut);
                }catch(Exception e)
                {
                    redisServer.IsConnectioning = false;
                    Console.WriteLine("connection error....");
                }
                             
            }

        }

        private void RedisClient_Connected(object sender, EventArgs e)
        {
            CSRedis.RedisClient redisClient = (CSRedis.RedisClient)sender;            
            RedisServer redisServer = RedisUtils.getRedisServer(redisClient);
            Console.WriteLine("connection success...");
            redisServer.IsConnectioning = false;
            redisServer.IsConnectioned = true;
            Console.WriteLine("name:" + redisServer.Connection.Name);
        }
    }
}
