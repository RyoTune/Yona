namespace Yona.Core.Settings.Models;

public record ColorTheme(string Name, string PrimaryColor, string AccentColor)
{
    public static readonly ColorTheme Yona = new("Yona", "#89d378", "#9900ff");
    public static readonly ColorTheme Custom = new("Custom", "#FFFFFF", "#000000");

    // SukiUI defaults.
    public static readonly ColorTheme Orange = new("Orange", "#d48806", "#176CE8");
    public static readonly ColorTheme Green = new("Green", "#537834", "#B24DB0");
    public static readonly ColorTheme Blue = new("Blue", "#0A59F7", "#F7A80A");
    public static readonly ColorTheme Red = new("Red", "#D03A2F", "#2FC5D0");
}