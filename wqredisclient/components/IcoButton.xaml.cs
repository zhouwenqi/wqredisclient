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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wqredisclient.components
{
    /// <summary>
    /// IcoButton.xaml 的交互逻辑
    /// </summary>
    public partial class IcoButton : Button
    {
        public static readonly DependencyProperty IcoProperty = DependencyProperty.Register("Ico", typeof(Geometry), typeof(IcoButton), new PropertyMetadata());
        public IcoButton()
        {
            InitializeComponent();
        }
        /// <summary>
        /// svg ico
        /// </summary>
        public Geometry Ico
        {
            set { SetValue(IcoProperty, value); }
            get { return (Geometry)GetValue(IcoProperty); }
        }

    }
}
