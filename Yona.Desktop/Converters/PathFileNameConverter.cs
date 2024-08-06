using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Globalization;
using System.IO;

namespace Yona.Desktop.Converters;

internal class PathFileNameConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string text) return Path.GetFileName(text);
        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
