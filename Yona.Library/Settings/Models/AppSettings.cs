using CommunityToolkit.Mvvm.ComponentModel;

namespace Yona.Library.Settings.Models;

public partial class AppSettings : ObservableObject
{
    [ObservableProperty]
    private bool useDarkMode = true;

    [ObservableProperty]
    private ThemeColor themeColor = ThemeColor.Orange;

    [ObservableProperty]
    private bool useAnimBackground = true;

    [ObservableProperty]
    private bool isMenuExpanded = true;

    [ObservableProperty]
    private PageType startPage = PageType.Home;
}
