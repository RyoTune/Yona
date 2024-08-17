using Yona.Core.Settings;

namespace Yona.Core.ViewModels;

public partial class MainWindowViewModel(SettingsService settings, UpdateService updates) : ViewModelBase
{
    public ViewModelBase? RootViewModel { get; set; }

    public SettingsService Settings { get; } = settings;

    public UpdateService Updates { get; } = updates;
}
