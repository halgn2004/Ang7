﻿using System.Globalization;
namespace Ang7.Helpers;

public class IntToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int intValue)
        {
            return intValue.ToString();
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (int.TryParse(value as string, out int result))
        {
            return result;
        }
        return 0; // Default value if parsing fails
    }
}