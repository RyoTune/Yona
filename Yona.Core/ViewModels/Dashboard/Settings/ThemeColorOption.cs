using Yona.Core.Settings.Models;

namespace Yona.Core.ViewModels.Dashboard.Settings;

public record ThemeColorOption(string Name, ThemeColor Color)
{
    public static readonly ThemeColorOption[] AvailableOptions =
    [
        new("Blue", ThemeColor.Blue),
        new("Red", ThemeColor.Red),
        new("Green", ThemeColor.Green),
        new("Orange", ThemeColor.Orange),
    ];
}