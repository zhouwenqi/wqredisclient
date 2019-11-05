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

namespace wqredisclient.window
{
    /// <summary>
    /// RedisServerWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RedisServerWindow : Window
    {
        public RedisConnection redisConnection;
        public RedisServerWindow()
        {
            InitializeComponent();
            if(null == redisConnection)
            {
                redisConnection = new RedisConnection();                
                this.Title = "add redis server";
            }else
            {
                this.Title = "edit redis server";
            }
            this.DataContext = redisConnection;
        }

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
            Console.WriteLine("init over...");
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
                App.config.RedisConnections.Add(redisConnection);
                RedisUtils.addConnection(redisConnection);               
                ConfigUtils.saveConfig();
                this.Close();
            }
            else
            {
                Console.WriteLine("false");
            }
        }
    }
}
