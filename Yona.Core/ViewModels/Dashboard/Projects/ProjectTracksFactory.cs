using Microsoft.Extensions.Logging;
using Yona.Core.Projects;
using Yona.Core.Projects.Models;
using Yona.Core.ViewModels.TrackPanel;

namespace Yona.Core.ViewModels.Dashboard.Projects;

public class ProjectTracksFactory(ProjectServices services, TrackPanelFactory trackPanel, ILogger<ProjectTracksViewModel> log)
{
    public ProjectTracksViewModel Create(ProjectsRouter router, ProjectBundle project)
        => new(router, project, services, trackPanel, log);
}
