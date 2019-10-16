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
    /// TopButton
    /// </summary>
    public partial class TopButton : Button
    {
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(Geometry), typeof(TopButton), new PropertyMetadata());
        public TopButton()
        {
            InitializeComponent();
        }

        /// <summary>
        /// svg icon
        /// </summary>
        public Geometry Icon
        {
            set { SetValue(IconProperty, value); }
            get { return (Geometry)GetValue(IconProperty); }
        }
    }
}
