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
    /// loading
    /// </summary>
    public partial class Loading : UserControl
    {
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(SolidColorBrush), typeof(Loading), new PropertyMetadata(Brushes.Red));
        public Loading()
        {
            InitializeComponent();
        }       
        public SolidColorBrush Fill
        {
            set
            {
                SetValue(FillProperty, value);
            }
            get
            {
                return (SolidColorBrush)GetValue(FillProperty);
            }
        }
    }
}
