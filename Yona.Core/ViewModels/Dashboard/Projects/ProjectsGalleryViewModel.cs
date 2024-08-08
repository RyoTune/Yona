using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using Yona.Core.Projects;
using Yona.Core.Projects.Models;

namespace Yona.Core.ViewModels.Dashboard.Projects;

public partial class ProjectsGalleryViewModel : ViewModelBase, IRoutableViewModel
{
    private readonly ProjectRepository projects;

    public ProjectsGalleryViewModel(IScreen host, ProjectRepository projects)
    {
        this.HostScreen = host;
        this.projects = projects;
    }

    public IReadOnlyList<ProjectBundle> Projects => this.projects.Items;

    public string? UrlPathSegment { get; } = "gallery";

    public IScreen HostScreen { get; }

    [RelayCommand]
    private void OpenProject(ProjectBundle project)
    {
        this.HostScreen.Router.Navigate.Execute(new ProjectTracksViewModel(this.HostScreen, project));
    }
}
