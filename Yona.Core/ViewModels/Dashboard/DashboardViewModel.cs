using Yona.Core.Settings;
using Yona.Core.Settings.Models;
using Yona.Core.ViewModels.Dashboard.Convert;
using Yona.Core.ViewModels.Dashboard.Home;
using Yona.Core.ViewModels.Dashboard.Projects;
using Yona.Core.ViewModels.Dashboard.Settings;

namespace Yona.Core.ViewModels.Dashboard;

public partial class DashboardViewModel : ViewModelBase
{
    private readonly SettingsService _settings;

    public DashboardViewModel(
        HomeViewModel home,
        ProjectsViewModel projects,
        ConvertViewModel convert,
        SettingsViewModel settings,
        SettingsService settingsService)
    {
        Home = home;
        Projects = projects;
        Convert = convert;
        Settings = settings;
        StartPage = settingsService.Current.StartPage;

        _settings = settingsService;
    }

    public HomeViewModel Home { get; }

    public ProjectsViewModel Projects { get; }

    public ConvertViewModel Convert { get; }

    public SettingsViewModel Settings { get; }

    public bool IsMenuExpanded
    {
        get => _settings.Current.IsMenuExpanded;
        set => SetProperty(_settings.Current.IsMenuExpanded, value, _settings.Current, (s, n) => s.IsMenuExpanded = value);
    }

    public Page StartPage { get; }
}
