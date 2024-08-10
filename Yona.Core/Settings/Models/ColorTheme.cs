namespace Yona.Core.Settings.Models;

public record ColorTheme(string Name, string PrimaryColor, string AccentColor)
{
    public static readonly ColorTheme[] AvailableOptions =
    [
        // SukiUI default themes.
        new("Orange", "#d48806", "#176CE8"),
        new("Red", "#D03A2F", "#2FC5D0"),
        new("Green", "#537834", "#B24DB0"),
        new("Blue", "#0A59F7", "#F7A80A"),
    ];
}