using Yona.Core.Settings;

namespace Yona.Core.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel(SettingsService settings)
    {
        Settings = settings;
    }

    public ViewModelBase? RootViewModel { get; set; }

    public SettingsService Settings { get; }
}
