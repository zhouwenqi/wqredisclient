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
using wqredisclient.components;
using Newtonsoft.Json;

namespace wqredisclient.window
{
    /// <summary>
    /// main window
    /// </summary>
    public partial class MainWindow : Window
    {
        private log4net.ILog log = log4net.LogManager.GetLogger("MainWindow.xaml.cs");
        private TreeViewItem selectDatabaseItem;        
        public RedisDatabase currentRedisDatabase;
        public RedisKey currentRedisKey;

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
            App.config.RedisConnections.ForEach((connection) => {                
                RedisUtils.addConnection(connection);
            });    
            for(int i=0;i<App.redisServers.Count;i++)
            {
                App.redisServers[i].RedisClient.Connected += RedisClient_Connected;
            }

            redisServerBox.Items.Clear();
            this.Dispatcher.Invoke(new Action(delegate
            {
                redisServerBox.ItemsSource = App.redisServers;
            }));

            
        }

        /// <summary>
        /// redis server select-item event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void redisServerBox_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            selectDatabaseItem = (TreeViewItem)redisServerBox.ItemContainerGenerator.ContainerFromItem(e.NewValue);
            if (e.NewValue.GetType() == typeof(RedisServer))
            {
                RedisServer redisServer = (RedisServer)e.NewValue;
                if(redisServer == null)
                {
                    return;
                }
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
                this.currentRedisDatabase = (RedisDatabase)e.NewValue;
                ThreadStart threadStart = new ThreadStart(() =>
                {
                    getKeys(currentRedisDatabase);
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
            // redisClient.Connected += RedisClient_Connected;     
            if (!redisClient.IsConnected)
            {
                try
                {
                    int connectionTimeOut = Convert.ToInt32(redisServer.Connection.ConnectionTimeOut);
                    redisClient.Connect(connectionTimeOut);
                }catch(Exception e)
                {
                    log.Info(e.Message);
                    redisServer.IsConnectioning = false;
                }                             
            }

        }

        /// <summary>
        /// get keys list
        /// </summary>
        /// <param name="redisDatabase"></param>
        private void getKeys(RedisDatabase redisDatabase)
        {                     
            CSRedis.RedisClient redisClient = redisDatabase.ParentServer.RedisClient;
            redisClient.Call("SELECT "+redisDatabase.Id);
            string[] keys = redisClient.Keys(redisDatabase.ParentServer.Connection.KeyPattern);
            char[] splits = redisDatabase.ParentServer.Connection.KeySeparator.ToCharArray();
            redisDatabase.KeyCount = keys.Length;
            ObservableCollection<RedisKey> redisKeys = new ObservableCollection<RedisKey>();
            if (keys.Length > 0)
            {
                Array.Sort(keys);
                redisKeys = RedisKeyUtils.getSplitKeys(keys,null);
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
            if (!string.IsNullOrEmpty(redisServer.Connection.Auth))
            {
                redisClient.Auth(redisServer.Connection.Auth);
            }
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

        private void redisKeysBox_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            RedisKey redisKey = (RedisKey)e.NewValue;
            if(redisKey == null)
            {
                return;
            }
            if (redisKey.Keys.Count == 0)
            {                
                ThreadStart threadStart = new ThreadStart(() =>
                {
                    getKeyValue(redisKey);
                });
                new Thread(threadStart).Start();
            }
        }
        private void getKeyValue(RedisKey redisKey)
        {
            currentRedisKey = redisKey;
            CSRedis.RedisClient redisClient = currentRedisDatabase.ParentServer.RedisClient;            
            string value = redisClient.Get(currentRedisKey.Key);
            this.inputKey.Dispatcher.Invoke(new Action(delegate
            {
                inputKey.Text = currentRedisKey.Key;
            }));
            this.inputValue.Dispatcher.Invoke(new Action(delegate
            {
                inputValue.Text = value;
            }));
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(inputKey.VaildStatus != InputVaildStatus.No)
            {
                inputKey.VaildStatus = InputVaildStatus.No;
            }else
            {
                inputKey.VaildStatus = InputVaildStatus.Yes;
            }
            
        }

        /// <summary>
        /// TopButton event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TopButton_Click(object sender, RoutedEventArgs e)
        {
            TopButton topButton = (TopButton)sender;
            if(null == topButton)
            {
                return;
            }
            switch (topButton.Name)
            {
                case "btnAddConnection":
                    RedisServerWindow redisServerWindow = new RedisServerWindow();
                    redisServerWindow.ShowDialog();
                    break;
            }
        }

        /// <summary>
        /// server context-menu item click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuElement_Server_Click(object sender, RoutedEventArgs e)
        {            
            MenuElement item = (MenuElement)sender;
            ContextMenu menu = (ContextMenu)item.Parent;
            string uid = (string)menu.Tag;
            RedisServer redisServer = RedisUtils.getRedisServer(uid);
            if(null == redisServer)
            {
                return;
            }
            TreeViewItem viewItem = (TreeViewItem)redisServerBox.ItemContainerGenerator.ContainerFromItem(redisServer);
            switch (item.Header.ToString())
            {
                case "Connection":
                case "Reload":                    
                    redisConnecton(redisServer);
                    break;
                case "Disconnection":
                    if (redisServer.RedisClient.IsConnected)
                    {
                        redisServer.RedisClient.Quit();
                        Console.WriteLine("status:" + redisServer.RedisClient.IsConnected);
                        redisServer.IsConnectioned = redisServer.RedisClient.IsConnected;
                    }
                    redisServer.Databases.Clear();
                    break;
                default:
                    break;
                
            }
        }
        private void redisRestconnection(RedisServer redisServer)
        {
            redisServer.IsConnectioning = true;
            if (redisServer.RedisClient.IsConnected)
            {
                redisServer.RedisClient.Quit();
            }
           
            ThreadStart threadStart = new ThreadStart(() =>
            {
                redisConnecton(redisServer);
            });
            new Thread(threadStart).Start();
        }

        private void databaseMenu_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string uid = e.Parameter.ToString();
            RedisServer redisServer = RedisUtils.getRedisServer(uid);
            if(null != redisServer)
            {
                if (redisServer.RedisClient.IsConnected)
                {
                    redisServer.RedisClient.Dispose();
                }
                redisConnecton(redisServer);
            }
        }
        
    }
}
