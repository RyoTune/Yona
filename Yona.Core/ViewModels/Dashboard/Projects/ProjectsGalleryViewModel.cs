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

    public ProjectsGalleryViewModel(IScreen host, ProjectRepository projects, TrackPanelFactory trackPanel)
    {
        this.HostScreen = host;
        this.projects = projects;
        this.trackPanel = trackPanel;
    }

    public IReadOnlyList<ProjectBundle> Projects => this.projects.Items;

    public string? UrlPathSegment { get; } = "gallery";

    public IScreen HostScreen { get; }

    [RelayCommand]
    private void OpenProject(ProjectBundle project)
    {
        this.HostScreen.Router.Navigate.Execute(new ProjectTracksViewModel(this.HostScreen, project, trackPanel));
    }
}
