using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Globalization;
using Yona.Desktop.Common;
using Yona.Library.Projects.Models;

namespace Yona.Desktop.Converters;

internal class ProjectIconConverter : IValueConverter
{
    private static readonly Uri appIcon = new("avares://Yona.Desktop/Assets/Icons/logo-icon.webp");

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Project project)
        {
            return null;
        }
        else if (value is ProjectTemplate template)
        {
            if (IconUtils.GetDefaultIconFromName(template.Name) is string internalName)
            {
                var iconUri = new Uri($"avares://Yona.Desktop/Assets/Icons/{internalName}.webp");

                try
                {
                    return new Bitmap(AssetLoader.Open(iconUri));
                }
                catch (Exception)
                {
                }
            }

            return new Bitmap(AssetLoader.Open(appIcon));
        }

        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return new NotSupportedException();
    }
}
