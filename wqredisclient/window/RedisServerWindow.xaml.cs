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
using wqredisclient.components;
using System.Threading;
using CSRedis;

namespace wqredisclient.window
{
    /// <summary>
    /// RedisServerWindow.xaml
    /// </summary>
    public partial class RedisServerWindow : Window
    {
        private RedisConnection redisConnection;
        private bool isNew = true;
        private RedisClient testClient;
        private log4net.ILog log = log4net.LogManager.GetLogger("RedisServerWindow.xaml.cs");
        public RedisServerWindow()
        {
            InitializeComponent();
            redisConnection = new RedisConnection();
        }
        public RedisServer Server { get; set; }

        /// <summary>
        /// click cancel button event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// window load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Server == null)
            {
                this.Title = "add redis server";
            }
            else
            {
                isNew = false;
                this.Title = "edit redis server";
                redisConnection = (RedisConnection)Server.Connection.Clone();
            }
            this.DataContext = redisConnection;
        }

        /// <summary>
        /// click save button event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            saveRedisServer();
        }

        /// <summary>
        /// save redis server
        /// </summary>
        private void saveRedisServer()
        {
            if(CompontentUtils.vaildInputBox(name)
                && CompontentUtils.vaildInputBox(host)
                && CompontentUtils.vaildInputBox(port)
                && CompontentUtils.vaildInputBox(keyPattern)
                && CompontentUtils.vaildInputBox(keySeparator)
                && CompontentUtils.vaildInputBox(connectionTimeout)
                && CompontentUtils.vaildInputBox(executeTimeout))
            {
                
                if (isNew)
                {
                    App.config.RedisConnections.Add(redisConnection);
                    RedisUtils.addConnection(redisConnection);
                }else
                {
                    RedisUtils.updateConnection(Server, redisConnection);
                }
                ConfigUtils.updateConfig();
                this.Close();
            }
            else
            {
                Console.WriteLine("false");
            }
        }

        /// <summary>
        /// test connection event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            if (CompontentUtils.vaildInputBox(host) && CompontentUtils.vaildInputBox(port) && CompontentUtils.vaildInputBox(connectionTimeout))
            {
                string host = redisConnection.Host;
                int port = Convert.ToInt32(redisConnection.Port);
                int timeout = Convert.ToInt32(redisConnection.ConnectionTimeOut);
                testClient = new RedisClient(host, port);
                testClient.Connected += TestClient_Connected;
                ThreadStart threadStart = new ThreadStart(()=> {
                    testConnection(timeout);
                });
                new Thread(threadStart).Start();
            }
            
        }
        /// <summary>
        /// test connection
        /// </summary>
        /// <param name="timeout"></param>
        private void testConnection(int timeout)
        {
            this.Dispatcher.Invoke(new Action(delegate
            {
                btnTest.IsEnabled = false;
                btnTest.IsLoading = true;

            }));
            try
            {
                testClient.Connect(timeout);
            }
            catch(Exception e)
            {
                log.Error(e.Message);
                this.Dispatcher.Invoke(new Action(delegate
                {
                    btnTest.IsLoading = false;
                    btnTest.IsEnabled = true;

                }));
                MessageBox.Show("failed", "test connection failed...", MessageBoxButton.OK,MessageBoxImage.Warning);
            }
        }
        /// <summary>
        /// connected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestClient_Connected(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(new Action(delegate
            {
                btnTest.IsLoading = false;
                btnTest.IsEnabled = true;

            }));
            if (testClient.IsConnected)
            {
                MessageBox.Show("successful connection", "successful connection to redis-server", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("failed", "test connection failed...", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        
    }
}
