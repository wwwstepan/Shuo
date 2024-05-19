using System.Globalization;

namespace Shuo;

public class IndexFrom1Converter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
        => (value is int i) ? i + 1 : 0;
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => (value is int i) ? i - 1: 0;
}

