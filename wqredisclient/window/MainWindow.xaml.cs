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

            string[] keys = { "423424", "234smfls", "234smfls:333","win:4234","windoww", "3324:44", "9849234", "win:4423" };
            RedisKeyUtils.getRedisKeys(keys);
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
            char[] splits = new char[] { ':', '#', '=' };
           
            CSRedis.RedisClient redisClient = redisDatabase.ParentServer.RedisClient;
            redisClient.Call("SELECT "+redisDatabase.Id);
            string[] keys = redisClient.Keys("*");
            redisDatabase.KeyCount = keys.Length;
            ObservableCollection<RedisKey> redisKeys = new ObservableCollection<RedisKey>();
            if (keys.Length > 0)
            {
                string mmt = "file:open:excel:384982&file:open:excel:884982&file:open:word:444234&file:open:word:998402&file:save:excel:4444&file:save:excel:3333&file:delete:4444&file:delete:9999&file:oi&edit:open:3333&edit:query:4444&project:5555&project:33333&view&window";
                keys = mmt.Split('&');
                redisKeys = RedisKeyUtils.getSplitStr(keys);
            }            
            this.Dispatcher.Invoke(new Action(delegate
            {
                redisKeysBox.ItemsSource = redisKeys;
            }));

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
    }
}
