using System.Globalization;

namespace Ang7.Helpers;

public class ArabicNumbersConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return GlobalFunc.ConvertNumberToAr(value.ToString());
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }

}

