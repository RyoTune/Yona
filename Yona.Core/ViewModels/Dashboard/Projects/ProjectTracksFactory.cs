using Yona.Core.Projects;
using Yona.Core.Projects.Models;
using Yona.Core.ViewModels.TrackPanel;

namespace Yona.Core.ViewModels.Dashboard.Projects;

public class ProjectTracksFactory(ProjectServices services, TrackPanelFactory trackPanel)
{
    private readonly ProjectServices services = services;
    private readonly TrackPanelFactory trackPanel = trackPanel;

    public ProjectTracksViewModel Create(ProjectsRouter router, ProjectBundle project)
        => new(router, project, this.services, this.trackPanel);
}
