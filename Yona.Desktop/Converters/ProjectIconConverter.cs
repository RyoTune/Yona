using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Globalization;
using System.IO;
using Yona.Core.Projects.Models;
using Yona.Desktop.Common;

namespace Yona.Desktop.Converters;

internal class ProjectIconConverter : IValueConverter
{
    private static readonly Uri appIcon = new("avares://Yona.Desktop/Assets/Icons/logo-icon.webp");

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is ProjectBundle project)
        {
            if (File.Exists(project.IconFile))
            {
                try
                {
                    return new Bitmap(project.IconFile);
                }
                catch (Exception) { }
            }
            else if (IconUtils.GetDefaultIconFromName(project.Data.Name) is string knownName)
            {
                var iconUri = new Uri($"avares://Yona.Desktop/Assets/Icons/{knownName}.webp");

                try
                {
                    return new Bitmap(AssetLoader.Open(iconUri));
                }
                catch (Exception) { }
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
