using System;
using System.Globalization;
using System.Windows.Data;

namespace DispoDataAssistant.Converters
{
    public class BoolToTranslateTransformYConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? 0 : 500;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
