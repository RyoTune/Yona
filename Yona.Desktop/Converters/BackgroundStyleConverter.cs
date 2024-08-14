using Avalonia.Data.Converters;
using System;
using System.Globalization;
using Yona.Core.Settings.Models;

namespace Yona.Desktop.Converters;

internal class BackgroundStyleConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is BackgroundStyle style)
        {
            switch (style.Mode)
            {
                case BackgroundStyleMode.Gradient:
                    return SukiUI.Enums.SukiBackgroundStyle.Gradient;
                case BackgroundStyleMode.Bubble:
                    return SukiUI.Enums.SukiBackgroundStyle.Bubble;
                case BackgroundStyleMode.BubbleStrong:
                    return SukiUI.Enums.SukiBackgroundStyle.BubbleStrong;
                case BackgroundStyleMode.Flat:
                    return SukiUI.Enums.SukiBackgroundStyle.Flat;
            }
        }

        return SukiUI.Enums.SukiBackgroundStyle.BubbleStrong;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
