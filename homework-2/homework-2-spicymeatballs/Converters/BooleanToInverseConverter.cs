using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace homework_2_spicymeatballs.Converters
{
    public class BooleanToInverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue; // Return the inverse of the boolean value
            }
            return false; // Return false by default if value is not a boolean
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // No need to implement ConvertBack for this case
        }
    }
}