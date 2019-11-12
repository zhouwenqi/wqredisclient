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
    /// IcoButton.xaml
    /// </summary>
    public partial class IcoButton : Button
    {
        public static readonly DependencyProperty IcoProperty = DependencyProperty.Register("Ico", typeof(Geometry), typeof(IcoButton), new PropertyMetadata());
        public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register("IsLoading", typeof(bool), typeof(IcoButton), new PropertyMetadata(false));        
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
        /// <summary>
        /// loading status
        /// </summary>
        public bool IsLoading
        {
            set
            {
                SetValue(IsLoadingProperty, value);
            }
            get
            {
                return (bool)GetValue(IsLoadingProperty);
            }
        }

    }
}
