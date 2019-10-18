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
    public class FillConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
            {
                return DependencyProperty.UnsetValue;
            }
            bool isSuccess = (bool)value;
            SolidColorBrush primaryBrush = (SolidColorBrush)Application.Current.FindResource("primaryBrush");
            SolidColorBrush icoLightBrush = (SolidColorBrush)Application.Current.FindResource("icoNormalBrush");
            return isSuccess ? primaryBrush : icoLightBrush;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }
            SolidColorBrush brush = (SolidColorBrush)value;
            return brush.Color.R == 96 ? true : false;
        }

    }
}
