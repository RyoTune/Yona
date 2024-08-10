using Yona.Core.Audio;
using Yona.Core.Projects;
using Yona.Core.Projects.Models;
using Yona.Core.ViewModels.TrackPanel;

namespace Yona.Core.ViewModels.Dashboard.Projects;

public class ProjectTracksFactory(ProjectBuilder builder, EncoderRepository encoders, TrackPanelFactory trackPanel, TemplateRepository templates)
{
    private readonly ProjectBuilder builder = builder;
    private readonly EncoderRepository encoders = encoders;
    private readonly TrackPanelFactory trackPanel = trackPanel;
    private readonly TemplateRepository templates = templates;

    public ProjectTracksViewModel Create(ProjectsRouter router, ProjectBundle project)
        => new(router, project, this.templates, this.encoders, this.builder, this.trackPanel);
}
