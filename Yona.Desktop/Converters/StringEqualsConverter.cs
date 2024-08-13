using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Yona.Desktop.Converters;

internal class StringEqualsConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string str1 && parameter is string str2)
        {
            return str1 == str2;
        }

        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
