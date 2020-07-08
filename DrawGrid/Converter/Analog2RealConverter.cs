using System;
using System.Globalization;
using System.Windows.Data;

namespace DrawGrid.Converter
{
    public class Analog2RealConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (null != value && value is double status)
            {
                return 5 * status;
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}