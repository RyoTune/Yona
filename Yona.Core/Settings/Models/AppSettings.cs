using CommunityToolkit.Mvvm.ComponentModel;

namespace Yona.Core.Settings.Models;

public partial class AppSettings : ObservableObject
{
    [ObservableProperty]
    private ColorTheme themeColor = ColorTheme.AvailableOptions.First();

    [ObservableProperty]
    private ThemeMode themeMode = ThemeMode.Auto;

    [ObservableProperty]
    private bool useAnimBackground = true;

    [ObservableProperty]
    private bool isMenuExpanded = true;

    [ObservableProperty]
    private Page startPage = Page.Home;

    [ObservableProperty]
    private bool isDevMode;
}
