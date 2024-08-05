using Yona.Library.Projects.Models;

namespace Yona.Library.ViewModels.Dashboard.Home;

public class HomeViewModel : ViewModelBase
{
    public HomeViewModel()
    {
    }

    public List<Project> Projects { get; init; } = [];

    public List<Project> RecentProjects => this.Projects.Take(10).ToList();

    public List<ProjectTemplate> Templates { get; init; } = [];
}
