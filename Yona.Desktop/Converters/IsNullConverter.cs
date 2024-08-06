using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Yona.Desktop.Converters;

internal class IsNullConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value == null;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
