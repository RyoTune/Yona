using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using Yona.Core.Projects;
using Yona.Core.Projects.Models;
using Yona.Core.ViewModels.TrackPanel;

namespace Yona.Core.ViewModels.Dashboard.Projects;

public partial class ProjectsGalleryViewModel : ViewModelBase, IRoutableViewModel
{
    private readonly ProjectRepository projects;
    private readonly TrackPanelFactory trackPanel;
    private readonly ProjectBuilder builder;

    public ProjectsGalleryViewModel(
        IScreen host,
        ProjectRepository projects,
        TrackPanelFactory trackPanel,
        ProjectBuilder builder)
    {
        this.HostScreen = host;
        this.projects = projects;
        this.trackPanel = trackPanel;
        this.builder = builder;
    }

    public IReadOnlyList<ProjectBundle> Projects => this.projects.Items;

    public string? UrlPathSegment { get; } = "gallery";

    public IScreen HostScreen { get; }

    [RelayCommand]
    private void OpenProject(ProjectBundle project)
    {
        this.HostScreen.Router.Navigate.Execute(new ProjectTracksViewModel(this.HostScreen, project, this.trackPanel, this.builder));
    }
}
