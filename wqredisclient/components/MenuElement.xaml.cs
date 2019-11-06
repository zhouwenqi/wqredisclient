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
    /// MenuElement 
    /// </summary>
    public partial class MenuElement : MenuItem
    {
        public static readonly DependencyProperty SvgIconProperty = DependencyProperty.Register("SvgIcon", typeof(Geometry), typeof(MenuElement), new PropertyMetadata());
        public static readonly DependencyProperty IcoBrushProperty = DependencyProperty.Register("IcoBrush", typeof(SolidColorBrush), typeof(MenuElement), new PropertyMetadata((SolidColorBrush)App.Current.FindResource("icoNormalBrush")));
        public MenuElement()
        {
            InitializeComponent();
        }
        /// <summary>
        /// svg icon
        /// </summary>
        public Geometry SvgIcon
        {
            set { SetValue(SvgIconProperty, value); }
            get { return (Geometry)GetValue(SvgIconProperty); }
        }
        public SolidColorBrush IcoBrush
        {
            get
            {                
                return (SolidColorBrush)GetValue(IcoBrushProperty);
            }
            set
            {                
                SetValue(IcoBrushProperty, value);
            }
        }
    }
}
