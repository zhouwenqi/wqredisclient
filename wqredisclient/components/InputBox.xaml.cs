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
using wqredisclient.common;
using System.Text.RegularExpressions;

namespace wqredisclient.components
{
    /// <summary>
    /// InputBox.xaml 的交互逻辑
    /// </summary>
    public partial class InputBox : UserControl
    {
        private Geometry ico;
        private InputVaildStatus vaildStatus;
        private bool isNeedInput = false;
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(InputBox), new FrameworkPropertyMetadata(null) { BindsTwoWayByDefault = true });
        public static readonly DependencyProperty ValueTypeProperty = DependencyProperty.Register("ValueType", typeof(InputValueType), typeof(InputBox), new PropertyMetadata(InputValueType.Text));
        public static readonly DependencyProperty DefaultValueProperty = DependencyProperty.Register("DefaultValue", typeof(string), typeof(InputBox), new FrameworkPropertyMetadata(null) { BindsTwoWayByDefault = true });
        public InputBox()
        {
            InitializeComponent();            
        }
        /// <summary>
        /// text content
        /// </summary>
        public String Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        /// <summary>
        /// default text
        /// </summary>
        public String DefaultValue
        {
            get
            {
                return (string)GetValue(DefaultValueProperty);
            }
            set
            {
                SetValue(DefaultValueProperty, value);
            }
        }
        /// <summary>
        /// title
        /// </summary>
        public String Title
        {
            get { return label.Content.ToString(); }
            set { label.Content = value; }
        }
        /// <summary>
        /// svg ico
        /// </summary>
        public Geometry Ico
        {
            get { return ico; }
            set { ico = value; icoPath.Data = ico;if (ico != null) { icoPath.Visibility = Visibility.Visible; } }
        }

        public bool IsNeedInput
        {
            get
            {
                return isNeedInput;
            }
            set
            {
                isNeedInput = value;
                setInputVaildStatus();
            }
        }
        public InputVaildStatus VaildStatus
        {
            get
            {
                return vaildStatus;
            }
            set
            {
                vaildStatus = value;
                setInputVaildStatus();
            }
        }
        public InputValueType ValueType
        {
            get{
                return (InputValueType)GetValue(ValueTypeProperty);
            }
            set
            {
                SetValue(ValueTypeProperty, value);                
            }
        }
        /// <summary>
        /// isReadOnly
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return this.input.IsReadOnly;
            }
            set
            {
                this.input.IsReadOnly = value;
            }
        }
        /// <summary>
        /// input change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void input_TextChanged(object sender, TextChangedEventArgs e)
        {            
            if (string.IsNullOrEmpty(input.Text.Trim()))
            {
                label.Visibility = Visibility.Visible;
            }
            else
            {
                label.Visibility = Visibility.Hidden;
            }            
        }

        /// <summary>
        /// vaild
        /// </summary>
        private void setInputVaildStatus()
        {
            if (isNeedInput)
            {
                icoRequired.Visibility = Visibility.Visible;
            }else
            {
                icoRequired.Visibility = Visibility.Collapsed;
            }
            switch (vaildStatus)
            {
                case InputVaildStatus.No:
                    Color redColor = (Color)App.Current.FindResource("redColor");
                    icoRequired.Fill = new SolidColorBrush(redColor);
                    break;
                default:
                    icoRequired.Fill = (SolidColorBrush)App.Current.FindResource("icoNormalBrush");
                    break;
            }
        }

        private void input_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (ValueType == InputValueType.Integer)
            {
                Regex re = new Regex("[^0-9]+");
                e.Handled = re.IsMatch(e.Text);
            }else if(ValueType == InputValueType.Number)
            {
                Regex re = new Regex("[^0-9.]+");
                e.Handled = re.IsMatch(e.Text);
            }
        }
    }
}
