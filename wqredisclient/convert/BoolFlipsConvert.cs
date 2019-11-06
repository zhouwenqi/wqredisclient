using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;

namespace wqredisclient.convert
{
    public class BoolFlipsConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
            {
                return DependencyProperty.UnsetValue;
            }
            bool isSuccess = value == null ? false : (bool)value;
            return !isSuccess;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }
            bool isSuccess = (bool)value;
            return !isSuccess;
        }

    }
}
