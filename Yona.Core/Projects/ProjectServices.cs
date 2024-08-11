using Yona.Core.Audio;
using Yona.Core.Projects.Builders;
using Yona.Core.Projects.Models;

namespace Yona.Core.Projects;

public class ProjectServices
{
    private readonly ProjectRepository projects;
    private readonly TemplateRepository templates;
    private readonly ProjectBuilder builder;
    private readonly EncoderRepository encoders;

    public ProjectServices(ProjectRepository projects, ProjectBuilder builder, TemplateRepository templates, EncoderRepository encoders)
    {
        this.projects = projects;
        this.builder = builder;
        this.templates = templates;
        this.encoders = encoders;
    }

    public string[] Encoders => this.encoders.Items.Select(x => x.Name).ToArray();

    public string[] Templates => this.templates.Items.Select(x => x.Data.Name).ToArray();

    public Task BuildProject(ProjectBundle project, IProgress<float>? progress = null) => this.builder.Build(project, progress);

    public void DeleteProject(ProjectBundle project) => this.projects.Delete(project);

    public void ResetProject(ProjectBundle project)
    {
        var templateProject = this.templates.Items.FirstOrDefault(x => x.Data.Name == project.Data.Template);
        if (templateProject != null)
        {
            project.Data.Tracks = templateProject.Data.Tracks;
            project.Save();
        }
    }
}
