using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Yona.Core.Projects;
using Yona.Core.Projects.Models;

namespace Yona.Core.ViewModels.Dashboard.Home;

public partial class HomeViewModel : ViewModelBase
{
    private readonly ProjectsRepository projects;
    private readonly TemplatesRepository templates;
    private readonly ILogger log;

    public HomeViewModel(ProjectsRepository projects, TemplatesRepository templates, ILogger log)
    {
        this.log = log;
        this.projects = projects;
        this.templates = templates;
    }

    public List<Project> Projects { get; init; } = [];

    public List<Project> RecentProjects => this.projects.Items.Take(10).ToList();

    public IReadOnlyList<Project> Templates => this.templates.Items;

    [RelayCommand]
    public void CreateProject(Project template)
    {
        try
        {
            this.projects.Add(template);
        }
        catch (Exception ex)
        {
            this.log.LogError(ex, "Failed to create new project.");
        }
    }
}
