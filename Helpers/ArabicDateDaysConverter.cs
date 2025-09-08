using System.Globalization;

namespace Ang7.Helpers;

public class ArabicDateDaysConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        switch (value.ToString().ToLower())
        {
            case "sat":
                return "السبت";
            case "sun":
                return "الأحد";
            case "mon":
                return "الأثنين";
            case "tue":
                return "الثلاث";
            case "wed":
                return "الأربع";
            case "thu":
                return "الخميس";
            case "fri":
                return "الجمعة";
            default:
                return "الجمعة";
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }

}

