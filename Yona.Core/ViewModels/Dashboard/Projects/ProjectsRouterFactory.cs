using ReactiveUI;
using Yona.Core.Projects;

namespace Yona.Core.ViewModels.Dashboard.Projects;

public class ProjectsRouterFactory(ProjectRepository projects, ProjectTracksFactory projectTracks)
{
    private readonly ProjectRepository projects = projects;
    private readonly ProjectTracksFactory projectTracks = projectTracks;

    public ProjectsRouter Create(IScreen host) => new(host, projects, projectTracks);
}
