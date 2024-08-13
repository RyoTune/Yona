using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using Yona.Core.Settings;
using Yona.Core.Settings.Models;

namespace Yona.Core.ViewModels.Dashboard.Settings;

public partial class SettingsViewModel : ViewModelBase
{
    public SettingsViewModel(SettingsService settings)
    {
        this.Settings = settings;
    }

    public SettingsService Settings { get; }

    public ThemeMode[] Themes { get; } = Enum.GetValues<ThemeMode>();

    public IEnumerable<ColorTheme> ColorOptions { get; }
        = [ ColorTheme.Yona, ColorTheme.Orange, ColorTheme.Red, ColorTheme.Green, ColorTheme.Blue ];

    public Page[] Pages { get; } = Enum.GetValues<Page>();

    public Page SelectedPage
    {
        get => this.Settings.Current.StartPage;
        set => this.SetProperty(this.Settings.Current.StartPage, value, this.Settings.Current, (s, n) => s.StartPage = n);
    }

    [RelayCommand]
    private void Reset() => this.Settings.Reset();
}
