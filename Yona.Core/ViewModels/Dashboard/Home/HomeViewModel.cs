using System.Collections.ObjectModel;
using Yona.Core.Projects;
using Yona.Core.Projects.Models;

namespace Yona.Core.ViewModels.Dashboard.Home;

public class HomeViewModel : ViewModelBase
{
    private readonly TemplatesRegistry _templates;

    public HomeViewModel(TemplatesRegistry templates)
    {
        _templates = templates;
    }

    public List<Project> Projects { get; init; } = [];

    public List<Project> RecentProjects => Projects.Take(10).ToList();

    public ObservableCollection<ProjectTemplate> Templates => _templates.Templates;
}
