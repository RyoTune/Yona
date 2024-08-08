using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Yona.Core.Projects;
using Yona.Core.Projects.Models;

namespace Yona.Core.ViewModels.Dashboard.Home;

public partial class HomeViewModel : ViewModelBase
{
    private readonly ProjectRepository projects;
    private readonly TemplateRepository templates;
    private readonly ILogger log;

    public HomeViewModel(ProjectRepository projects, TemplateRepository templates, ILogger log)
    {
        this.log = log;
        this.projects = projects;
        this.templates = templates;
    }

    public List<ProjectBundle> RecentProjects => this.projects.Items.Take(10).ToList();

    public IReadOnlyList<ProjectBundle> Templates => this.templates.Items;

    [RelayCommand]
    public void CreateProject(ProjectBundle template)
    {
        try
        {
            // TODO: Tracks are a reference collection here.
            // new projects will edit the tracks as the template's probably.
            // Have to clone first.
            var newProject = new ProjectData()
            {
                Name = template.Data.Name,
                Tracks = template.Data.Tracks,
            };

            this.projects.Create(newProject);
        }
        catch (Exception ex)
        {
            this.log.LogError(ex, "Failed to create new project.");
        }
    }
}
