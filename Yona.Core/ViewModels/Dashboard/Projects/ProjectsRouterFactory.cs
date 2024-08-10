using ReactiveUI;
using Yona.Core.Audio;
using Yona.Core.Projects;

namespace Yona.Core.ViewModels.Dashboard.Projects;

public class ProjectsRouterFactory(ProjectRepository projects, ProjectTracksFactory projectTracks, EncoderRepository encoders)
{
    private readonly ProjectRepository projects = projects;
    private readonly ProjectTracksFactory projectTracks = projectTracks;
    private readonly EncoderRepository encoders = encoders;

    public ProjectsRouter Create(IScreen host) => new(host, projects, encoders, projectTracks);
}
