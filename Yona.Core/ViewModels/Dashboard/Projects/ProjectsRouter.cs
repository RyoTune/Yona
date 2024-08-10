using ReactiveUI;
using Yona.Core.Audio;
using Yona.Core.Projects;
using Yona.Core.Projects.Models;

namespace Yona.Core.ViewModels.Dashboard.Projects;

public class ProjectsRouter : RoutingState
{
    private readonly ProjectTracksFactory projectTracks;
    private readonly ProjectsGalleryViewModel projectsGallery;

    public ProjectsRouter(IScreen host, ProjectRepository projects, EncoderRepository encoders, ProjectTracksFactory projectTracks)
    {
        this.projectTracks = projectTracks;
        this.HostScreen = host;
        this.projectsGallery = new ProjectsGalleryViewModel(this, projects);
        this.Navigate.Execute(this.projectsGallery);
    }

    public IScreen HostScreen { get; }

    public void OpenProject(ProjectBundle project)
    {
        this.Navigate.Execute(this.projectTracks.Create(this, project));
    }
}
