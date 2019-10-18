using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private TreeViewItem selectDatabaseItem;
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

        private void redisServerBox_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            selectDatabaseItem = (TreeViewItem)redisServerBox.ItemContainerGenerator.ContainerFromItem(e.NewValue);
            if (e.NewValue.GetType() == typeof(RedisServer))
            {
                RedisServer redisServer = (RedisServer)e.NewValue;
                if (!redisServer.RedisClient.IsConnected)
                {
                    redisServer.IsConnectioning = true;
                    ThreadStart threadStart = new ThreadStart(() =>
                    {
                        redisConnecton(redisServer);
                    });
                    new Thread(threadStart).Start();
                }
            }
            else if (e.NewValue.GetType() == typeof(RedisDatabase))
            {
                RedisDatabase redisDatabase = (RedisDatabase)e.NewValue;
                ThreadStart threadStart = new ThreadStart(() =>
                {
                    getKeys(redisDatabase);
                });
                new Thread(threadStart).Start();
            }            
        }

        /// <summary>
        /// redis connection
        /// </summary>
        /// <param name="redisServer"></param>
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
        private void getKeys(RedisDatabase redisDatabase)
        {
            CSRedis.RedisClient redisClient = redisDatabase.ParentServer.RedisClient;
            redisClient.Call("SELECT "+redisDatabase.Id);
            string[] keys = redisClient.Keys("*");
            redisDatabase.KeyCount = keys.Length;
            foreach(String key in keys)
            {
                Console.WriteLine("k:" + key);
            }
            
        }

        private void RedisClient_Connected(object sender, EventArgs e)
        {
            CSRedis.RedisClient redisClient = (CSRedis.RedisClient)sender;            
            RedisServer redisServer = RedisUtils.getRedisServer(redisClient);
            Console.WriteLine("connection success...");
            int dbCount = RedisUtils.getDatabasesCount(redisClient);
            ObservableCollection<RedisDatabase> databases = new ObservableCollection<RedisDatabase>();
            for (int i = 0; i < dbCount; i++)
            {
                string dbName = "db" + i;                
                databases.Add(new RedisDatabase() { Id = i, Name = dbName, ParentServer = redisServer });
            }
            redisServer.Databases = databases;
            redisServer.IsConnectioning = false;
            redisServer.IsConnectioned = true;
            this.Dispatcher.Invoke(new Action(delegate
            {
                selectDatabaseItem.IsExpanded = true;
            }));
        }

        private void checkTheme_Click(object sender, RoutedEventArgs e)
        {

        }

        private void checkLanguage_Click(object sender, RoutedEventArgs e)
        {
            ResourceDictionary resource = new ResourceDictionary();
            if ((bool)checkLanguage.IsChecked)
            {
                resource.Source = new Uri("/language/zh_CN.xaml", UriKind.Relative);
            }
            else
            {
                resource.Source = new Uri("/language/en_US.xaml", UriKind.Relative);
            } 
            Application.Current.Resources.MergedDictionaries[1] = resource;
        }

        private void redisServerBox_Selected(object sender, RoutedEventArgs e)
        {
            //selectDatabaseItem = (TreeViewItem)e.OriginalSource;
            //if (redisServerBox.SelectedItem.GetType() == typeof(RedisServer))
            //{
            //    RedisServer redisServer = (RedisServer)redisServerBox.SelectedItem;
            //    if (!redisServer.RedisClient.IsConnected)
            //    {
            //        redisServer.IsConnectioning = true;
            //        ThreadStart threadStart = new ThreadStart(() =>
            //        {
            //            redisConnecton(redisServer);
            //        });
            //        new Thread(threadStart).Start();
            //    }
            //}
            //else if (redisServerBox.SelectedItem.GetType() == typeof(RedisDatabase))
            //{
            //    RedisDatabase redisDatabase = (RedisDatabase)redisServerBox.SelectedItem;
            //}
        }
    }
}
