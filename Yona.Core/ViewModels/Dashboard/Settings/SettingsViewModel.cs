using CommunityToolkit.Mvvm.Input;
using Yona.Core.Settings;
using Yona.Core.Settings.Models;

namespace Yona.Core.ViewModels.Dashboard.Settings;

public partial class SettingsViewModel : ViewModelBase
{
    private ThemeColorOption _selectedColor;

    public SettingsViewModel(SettingsService settings)
    {
        this.Settings = settings;
        this._selectedColor = this.ColorOptions.First(x => x.Color == this.Settings.Current.ThemeColor);
    }

    public SettingsService Settings { get; }

    public ThemeMode[] Themes { get; } = Enum.GetValues<ThemeMode>();

    public ThemeMode SelectedTheme
    {
        get => this.Settings.Current.ThemeMode;
        set => this.SetProperty(this.Settings.Current.ThemeMode, value, this.Settings.Current, (s, n) => s.ThemeMode = n);
    }

    public ThemeColorOption[] ColorOptions { get; } = ThemeColorOption.AvailableOptions;

    public ThemeColorOption SelectedColor
    {
        get => this._selectedColor;
        set
        {
            this.SetProperty(ref this._selectedColor, value);
            this.Settings.Current.ThemeColor = this._selectedColor.Color;
        }
    }

    public Page[] Pages { get; } = Enum.GetValues<Page>();

    public Page SelectedPage
    {
        get => this.Settings.Current.StartPage;
        set => this.SetProperty(this.Settings.Current.StartPage, value, this.Settings.Current, (s, n) => s.StartPage = n);
    }

    [RelayCommand]
    private void Reset() => this.Settings.Reset();
}
