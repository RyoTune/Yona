using CommunityToolkit.Mvvm.ComponentModel;

namespace Yona.Core.Settings.Models;

public partial class AppSettings : ObservableObject
{
    private BackgroundStyle backgroundStyle = BackgroundStyle.BubbleStrong;

    [ObservableProperty]
    private ColorTheme themeColor = ColorTheme.Yona;

    [ObservableProperty]
    private string customPrimaryColor = ColorTheme.Custom.PrimaryColor;

    [ObservableProperty]
    private string customAccentColor = ColorTheme.Custom.AccentColor;

    [ObservableProperty]
    private ThemeMode themeMode = ThemeMode.Auto;

    public BackgroundStyle BackgroundStyle
    {
        get
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                return BackgroundStyle.Flat;
            }

            return this.backgroundStyle;
        }

        set => this.SetProperty(ref this.backgroundStyle, value);
    }

    [ObservableProperty]
    private bool useAnimBackground = true;

    [ObservableProperty]
    private bool isMenuExpanded = true;

    [ObservableProperty]
    private Page startPage = Page.Home;

    [ObservableProperty]
    private bool isDevMode;
}
