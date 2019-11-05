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
using wqredisclient.common;

namespace wqredisclient.convert
{
    public class ValueTypeBoolConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
            {
                return DependencyProperty.UnsetValue;
            }
            InputValueType valueType  = (InputValueType)value;
            return valueType == InputValueType.Text;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            InputValueType valueType = InputValueType.Text;
            if (value == null)
            {
                return valueType;
            }
            bool isInputMethodEnable = (bool)value;
            if (!isInputMethodEnable)
            {
                valueType = InputValueType.Number;
            }
            return valueType;
        }

    }
}
