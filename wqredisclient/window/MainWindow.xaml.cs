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
