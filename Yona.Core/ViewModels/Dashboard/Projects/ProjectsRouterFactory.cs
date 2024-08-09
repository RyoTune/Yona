using ReactiveUI;
using Yona.Core.Projects;
using Yona.Core.ViewModels.TrackPanel;

namespace Yona.Core.ViewModels.Dashboard.Projects;

public class ProjectsRouterFactory(ProjectRepository projects, TrackPanelFactory trackPanel, ProjectBuilder builder)
{
    private readonly ProjectRepository projects = projects;
    private readonly TrackPanelFactory trackPanel = trackPanel;
    private readonly ProjectBuilder builder = builder;

    public ProjectsRouter Create(IScreen host) => new(host, projects, trackPanel, builder);
}
