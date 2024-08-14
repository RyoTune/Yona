namespace Yona.Core.Settings.Models;

public record BackgroundStyle(string Name, BackgroundStyleMode Mode)
{
    public static readonly BackgroundStyle Gradient = new("Gradient", BackgroundStyleMode.Gradient);
    public static readonly BackgroundStyle Bubble = new("Bubble", BackgroundStyleMode.Bubble);
    public static readonly BackgroundStyle BubbleStrong = new("Bubble Strong", BackgroundStyleMode.BubbleStrong);
    public static readonly BackgroundStyle Flat = new("Flat", BackgroundStyleMode.Flat);
}

public enum BackgroundStyleMode
{
    Gradient,
    Bubble,
    BubbleStrong,
    Flat,
    ShaderFile
}
