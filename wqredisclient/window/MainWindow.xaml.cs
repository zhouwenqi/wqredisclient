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
using System.Diagnostics;

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
                    RedisKeyUtils.sp = redisServer.Connection.KeySeparator.ToCharArray();
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

            this.btnAddKey.Dispatcher.Invoke(new Action(delegate
            {
                this.btnAddKey.IsEnabled = false;
            }));
        }

        /// <summary>
        /// redis connection
        /// </summary>
        /// <param name="redisServer"></param>
        private void redisConnecton(RedisServer redisServer)
        {            
            CSRedis.RedisClient redisClient = redisServer.RedisClient;  
            if (!redisClient.IsConnected)
            {
                try
                {
                    int connectionTimeOut = Convert.ToInt32(redisServer.Connection.ConnectionTimeOut);
                    Debug.WriteLine("host:"+redisClient.Host+",port:" + redisClient.Port);
                    redisClient.Connected -= RedisClient_Connected;
                    redisClient.Connected += RedisClient_Connected;
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
                this.btnAddKey.IsEnabled = true;
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
            Debug.WriteLine("connection success...");
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
                if (selectDatabaseItem != null)
                {
                    selectDatabaseItem.IsExpanded = true;
                }                
            }));
        }


        /// <summary>
        /// set button enabled status
        /// </summary>
        /// <param name="isSelected"></param>
        private void setKeySelectStatus(bool isSelected)
        {  
            inputKey.IsEnabled = isSelected;
            viewType.IsEnabled = isSelected;
            inputValue.IsEnabled = isSelected;
            btnKeyGroup.IsEnabled = isSelected;
        }

        private void redisKeysBox_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {            
            RedisKey redisKey = (RedisKey)e.NewValue;
            if(redisKey == null)
            {
                currentRedisKey = null;
                setKeySelectStatus(false);
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
            try
            {
                CSRedis.RedisClient redisClient = currentRedisDatabase.ParentServer.RedisClient;
                string value = redisClient.Get(currentRedisKey.Key);
                this.Dispatcher.Invoke(new Action(delegate
                {
                    inputKey.Text = currentRedisKey.Key;
                    inputValue.Text = value;
                    setKeySelectStatus(true);
                }));
            }
            catch(Exception e)
            {
                log.Error(e.Message);
                setKeySelectStatus(false);
            }
            
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string key = inputKey.Text;
            string value = inputValue.Text;
            if (string.IsNullOrEmpty(key))
            {
                inputValue.VaildStatus = InputVaildStatus.No;
                return;
            }
            else
            {
                inputValue.VaildStatus = InputVaildStatus.None;
            }
            if (string.IsNullOrEmpty(value))
            {
                inputValue.VaildStatus = InputVaildStatus.No;
                return;
            }
            else
            {
                inputValue.VaildStatus = InputVaildStatus.None;
            }
            ThreadStart threadStart = new ThreadStart(() =>
            {
                string newKey = null;
                if (!key.Equals(currentRedisKey.Key))
                {
                    newKey = key;
                }
                saveKeyValue(currentRedisKey.Key, newKey, value);
            });
            new Thread(threadStart).Start();            
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
            viewItem.IsSelected = true;
            
            switch (item.Header.ToString())
            {
                case "Connection":
                case "Reload":
                    redisRestconnection(redisServer);
                    break;
                case "Disconnection":
                    redisQuitConnection(redisServer);
                    break;
                case "Delete":
                    redisQuitConnection(redisServer);
                    redisServer.RedisClient.Dispose();                  
                    App.redisServers.Remove(redisServer);                    
                    ConfigUtils.saveConfig();
                    break;
                case "Edit":
                    redisQuitConnection(redisServer);
                    RedisServerWindow editServerWindow = new RedisServerWindow();
                    RedisConnection conn = (RedisConnection)redisServer.Connection.Clone();
                    editServerWindow.Server = redisServer;                    
                    editServerWindow.ShowDialog();
                    break;                
                default:
                    break;                
            }
        }


        private void MenuElement_Box_Click(object sender, RoutedEventArgs e)
        {
            MenuElement menu = (MenuElement)sender;
            switch (menu.Header.ToString())
            {
                case "Add connection":
                    RedisServerWindow redisServerWindow = new RedisServerWindow();
                    redisServerWindow.ShowDialog();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// rest connection
        /// </summary>
        /// <param name="redisServer"></param>
        private void redisRestconnection(RedisServer redisServer)
        {
            redisServer.IsConnectioning = true;
            try
            {
                if (redisServer.RedisClient.IsConnected)
                {
                    redisServer.RedisClient.Quit();
                }
            }
            catch(Exception e)
            {
                log.Error("server connection quit error:"+e.Message);
            }
           
            ThreadStart threadStart = new ThreadStart(() =>
            {
                redisConnecton(redisServer);
            });
            new Thread(threadStart).Start();
        }
        /// <summary>
        /// quit connection
        /// </summary>
        /// <param name="redisServer"></param>
        private void redisQuitConnection(RedisServer redisServer)
        {
            try
            {
                if (redisServer.RedisClient.IsConnected)
                {
                    redisServer.RedisClient.Quit();
                    redisServer.Databases.Clear();
                }
            }
            catch (Exception e)
            {
                log.Error("server connection quit error:" + e.Message);
            }
            redisServer.IsConnectioned = redisServer.RedisClient.IsConnected;
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
        private void saveKeyValue(string key,string newKey,string value)
        {
            Debug.WriteLine("key:" + key + ",value:" + value);
            try
            {
                if (currentRedisDatabase == null || currentRedisKey == null)
                {
                    return;
                }
                CSRedis.RedisClient redisClient = currentRedisDatabase.ParentServer.RedisClient;
                redisClient.Call("SELECT " + currentRedisDatabase.Id);
                if (string.IsNullOrEmpty(newKey))
                {
                    redisClient.Set(key, value);                    
                }
                else
                {
                    redisClient.Rename(key, newKey);
                    redisClient.Set(newKey, value);
                    getKeys(currentRedisDatabase);
                }
                MessageBox.Show("set successfly", "success", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch (Exception e)
            {
                log.Error("save faild:" + e.Message);
            }
        }
        
    }
}
