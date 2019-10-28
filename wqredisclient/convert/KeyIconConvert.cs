using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;


namespace wqredisclient.convert
{
    class KeyIconConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return DependencyProperty.UnsetValue;
            }
            int count = (int)value;
            PathGeometry geometryFolder = (PathGeometry)Application.Current.FindResource("svgFolderOpen");
            PathGeometry geometryKey = (PathGeometry)Application.Current.FindResource("svgRedisKey");
            return count > 0 ? geometryKey : geometryFolder;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}
