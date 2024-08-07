using Yona.Core.Projects;
using Yona.Core.Projects.Models;

namespace Yona.Core.ViewModels.Dashboard.Home;

public class HomeViewModel : ViewModelBase
{
    private readonly TemplatesRepository templates;

    public HomeViewModel(TemplatesRepository templates)
    {
        this.templates = templates;
    }

    public List<Project> Projects { get; init; } = [];

    public List<Project> RecentProjects => Projects.Take(10).ToList();

    public IReadOnlyList<ProjectTemplate> Templates => this.templates.Items;
}
