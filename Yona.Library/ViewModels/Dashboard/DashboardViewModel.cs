using Yona.Library.Settings;
using Yona.Library.Settings.Models;
using Yona.Library.ViewModels.Dashboard.Convert;
using Yona.Library.ViewModels.Dashboard.Home;
using Yona.Library.ViewModels.Dashboard.Projects;
using Yona.Library.ViewModels.Dashboard.Settings;

namespace Yona.Library.ViewModels.Dashboard;

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
        this.Home = home;
        this.Projects = projects;
        this.Convert = convert;
        this.Settings = settings;
        this.StartPage = settingsService.Current.StartPage;

        this._settings = settingsService;
    }

    public HomeViewModel Home { get; }

    public ProjectsViewModel Projects { get; }

    public ConvertViewModel Convert { get; }

    public SettingsViewModel Settings { get; }

    public bool IsMenuExpanded
    {
        get => this._settings.Current.IsMenuExpanded;
        set => this.SetProperty(this._settings.Current.IsMenuExpanded, value, this._settings.Current, (s, n) => s.IsMenuExpanded = value);
    }

    public PageType StartPage { get; }
}
