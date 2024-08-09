using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using Yona.Core.Projects;
using Yona.Core.Projects.Models;

namespace Yona.Core.ViewModels.Dashboard.Projects;

public partial class ProjectsGalleryViewModel : ViewModelBase, IRoutableViewModel
{
    private readonly ProjectsRouter router;
    private readonly ProjectRepository projects;

    public ProjectsGalleryViewModel(ProjectsRouter router, ProjectRepository projects)
    {
        this.router = router;
        this.projects = projects;
    }

    public IReadOnlyList<ProjectBundle> Projects => this.projects.Items;

    public string? UrlPathSegment { get; } = "gallery";

    public IScreen HostScreen => this.router.HostScreen;

    [RelayCommand]
    private void OpenProject(ProjectBundle project) => this.router.OpenProject(project);
}
