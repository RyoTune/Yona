using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using Yona.Core.Projects;
using Yona.Core.Projects.Models;
using Yona.Core.ViewModels.TrackPanel;

namespace Yona.Core.ViewModels.Dashboard.Projects;

public class ProjectsRouter : RoutingState
{
    private readonly ProjectsGalleryViewModel projectsGallery;
    private readonly ProjectRepository projects;
    private readonly TrackPanelFactory trackPanel;
    private readonly ProjectBuilder builder;

    public ProjectsRouter(IScreen host, ProjectRepository projects, TrackPanelFactory trackPanel, ProjectBuilder builder)
    {
        this.projects = projects;
        this.trackPanel = trackPanel;
        this.builder = builder;

        this.HostScreen = host;
        this.projectsGallery = new ProjectsGalleryViewModel(this, projects);
        this.Navigate.Execute(this.projectsGallery);
    }

    public IScreen HostScreen { get; }

    public void OpenProject(ProjectBundle project)
    {
        this.Navigate.Execute(new ProjectTracksViewModel(this, project, this.builder, this.trackPanel));
    }
}
