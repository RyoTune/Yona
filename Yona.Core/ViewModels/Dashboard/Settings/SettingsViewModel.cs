using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Yona.Core.Settings;
using Yona.Core.Settings.Models;

namespace Yona.Core.ViewModels.Dashboard.Settings;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly UpdateService updates;

    [ObservableProperty]
    private bool _awaitingUpdate;

    public SettingsViewModel(SettingsService settings, UpdateService updates)
    {
        this.Settings = settings;
        this.updates = updates;
        this.CurrentVersion = this.updates.CurrentVersion.ToString(3);
    }

    public SettingsService Settings { get; }

    public string CurrentVersion { get; }

    public ThemeMode[] Themes { get; } = Enum.GetValues<ThemeMode>();

    public IEnumerable<ColorTheme> ColorOptions { get; } =
    [
        ColorTheme.Yona,
        ColorTheme.Orange,
        ColorTheme.Red,
        ColorTheme.Green,
        ColorTheme.Blue,
        ColorTheme.Custom
    ];

    public IEnumerable<BackgroundStyle> BackgroundOptions { get; } =
    [
        BackgroundStyle.Flat,
        BackgroundStyle.Gradient,
        BackgroundStyle.Bubble,
        BackgroundStyle.BubbleStrong,
    ];

    public Page[] Pages { get; } = Enum.GetValues<Page>();

    public Page SelectedPage
    {
        get => this.Settings.Current.StartPage;
        set => this.SetProperty(this.Settings.Current.StartPage, value, this.Settings.Current, (s, n) => s.StartPage = n);
    }

    public string CustomPrimaryColor
    {
        get => this.Settings.Current.CustomPrimaryColor;
        set => this.SetProperty(this.Settings.Current.CustomPrimaryColor, value, this.Settings.Current, (m, n) => m.CustomPrimaryColor = n);
    }

    public string CustomAccentColor
    {
        get => this.Settings.Current.CustomAccentColor;
        set => this.SetProperty(this.Settings.Current.CustomAccentColor, value, this.Settings.Current, (m, n) => m.CustomAccentColor = n);
    }

    [RelayCommand]
    private async Task CheckUpdates()
    {
        this.AwaitingUpdate = true;
        await Task.Delay(5000);
        await this.updates.CheckUpdates(true);
        this.AwaitingUpdate = false;
    }

    [RelayCommand]
    private void Reset() => this.Settings.Reset();
}
