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
            List<RedisConnection> redisConnections = new List<RedisConnection>();

            RedisConnection redis1 = new RedisConnection();
            RedisServer redisServer1 = new RedisServer();
            redisServer1.Host = "192.168.1.51";
            redisServer1.Name = "192.168.1.51";
            redisServer1.Port = 6379;          
            redisServer1.Databases.Add(new RedisDatabase { Name = "db-0" });
            redisServer1.Databases.Add(new RedisDatabase { Name = "db-1" });
            redisServer1.Databases.Add(new RedisDatabase { Name = "db-2" });
            redisServer1.Databases.Add(new RedisDatabase { Name = "db-3" });
            redisServer1.Databases.Add(new RedisDatabase { Name = "db-4" });
            redisServer1.Databases.Add(new RedisDatabase { Name = "db-5" });
            redisServer1.Databases.Add(new RedisDatabase { Name = "db-6" });
            redisServer1.Databases.Add(new RedisDatabase { Name = "db-7" });
            redisServer1.Databases.Add(new RedisDatabase { Name = "db-8" });
            redisServer1.Databases.Add(new RedisDatabase { Name = "db-9" });
            redis1.Server = redisServer1;

            RedisConnection redis2 = new RedisConnection();
            RedisServer redisServer2 = new RedisServer();
            redisServer2.Host = "192.168.3.83";
            redisServer2.Name = "192.168.3.83";
            redisServer2.Port = 6379;
            redis2.Server = redisServer2;

            RedisConnection redis3 = new RedisConnection();
            RedisServer redisServer3 = new RedisServer();
            redisServer3.Host = "192.168.3.220";
            redisServer3.Name = "localhost";
            redisServer3.Port = 6379;
            redis3.Server = redisServer3;

            redisConnections.Add(redis1);
            redisConnections.Add(redis2);
            redisConnections.Add(redis3);

            App.config.RedisConnections = redisConnections;
            redisServerBox.Items.Clear();
            this.Dispatcher.Invoke(new Action(delegate
            {
                redisServerBox.ItemsSource = App.config.RedisConnections;
            }));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ResourceDictionary resource = new ResourceDictionary();
            resource.Source = new Uri("/language/zh_CN.xaml", UriKind.Relative);
            Console.WriteLine("size:" + Application.Current.Resources.MergedDictionaries.Count);
            Application.Current.Resources.MergedDictionaries[1] = resource;
        }
    }
}
