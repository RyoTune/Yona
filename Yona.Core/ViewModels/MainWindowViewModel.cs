using Yona.Core.Settings;

namespace Yona.Core.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel(SettingsService settings, UpdateService updates)
    {
        Settings = settings;
        Updates = updates;
    }

    public ViewModelBase? RootViewModel { get; set; }

    public SettingsService Settings { get; }

    public UpdateService Updates { get; }
}
