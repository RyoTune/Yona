using ReactiveUI;
using System.Reactive.Disposables;
using Yona.Core.Projects;

namespace Yona.Core.ViewModels.Dashboard.Projects;

public partial class ProjectsViewModel : ViewModelBase, IActivatableViewModel, IScreen
{
    private readonly ProjectsGalleryViewModel projectsGallery;

    public ProjectsViewModel(ProjectRepository projects)
    {
        this.projectsGallery = new ProjectsGalleryViewModel(this, projects);
        this.Router.Navigate.Execute(this.projectsGallery);

        this.WhenActivated((CompositeDisposable disposables) =>
        {
        });
    }

    public ViewModelActivator Activator { get; } = new();

    public RoutingState Router { get; } = new();
}
