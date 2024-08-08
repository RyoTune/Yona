using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Yona.Desktop.Converters;

internal class ZeroAsNullConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int number)
        {
            return (number == 0) ? null : number;
        }

        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return 0;
        }

        return value;
    }
}
