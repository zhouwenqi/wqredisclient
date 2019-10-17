using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Shapes;
using System.Windows;

namespace wqredisclient.convert
{
    public class VisibleConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
            {
                return DependencyProperty.UnsetValue;
            }
            bool isVisable = (bool)value;
            return isVisable ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }
            Visibility visible = (Visibility)value;
            return visible == Visibility.Visible ? true : false;
        }

    }
}
