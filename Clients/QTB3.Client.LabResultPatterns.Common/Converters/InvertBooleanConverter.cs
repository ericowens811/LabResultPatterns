using System;
using System.Globalization;
using Xamarin.Forms;

namespace QTB3.Client.LabResultPatterns.Common.Converters
{
    // used in xaml binding expressions; see Pagebar
    public class InvertBooleanConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}
