using CommunityToolkit.Mvvm.Input;
using Yona.Library.Settings;
using Yona.Library.Settings.Models;

namespace Yona.Library.ViewModels.Dashboard.Settings;

public partial class SettingsViewModel : ViewModelBase
{
    private ThemeColorOption _selectedColor;

    public SettingsViewModel(SettingsService settings)
    {
        this.Settings = settings;
        this._selectedColor = this.ColorOptions.First(x => x.Color == this.Settings.Current.ThemeColor);
    }

    public SettingsService Settings { get; }

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

    public PageType[] Pages { get; } = Enum.GetValues<PageType>();

    public PageType SelectedPage
    {
        get => this.Settings.Current.StartPage;
        set => this.SetProperty(this.Settings.Current.StartPage, value, this.Settings.Current, (s, n) => s.StartPage = n);
    }

    [RelayCommand]
    private void Reset() => this.Settings.Reset();
}
