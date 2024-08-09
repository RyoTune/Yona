using ReactiveUI;
using System.Reactive.Disposables;
using Yona.Core.Projects;
using Yona.Core.ViewModels.TrackPanel;

namespace Yona.Core.ViewModels.Dashboard.Projects;

public partial class ProjectsViewModel : ViewModelBase, IActivatableViewModel, IScreen
{
    private readonly ProjectsGalleryViewModel projectsGallery;

    public ProjectsViewModel(ProjectRepository projects, TrackPanelFactory trackPanel, ProjectBuilder builder)
    {
        this.projectsGallery = new ProjectsGalleryViewModel(this, projects, trackPanel, builder);
        this.Router.Navigate.Execute(this.projectsGallery);

        this.WhenActivated((CompositeDisposable disposables) =>
        {
        });
    }

    public ViewModelActivator Activator { get; } = new();

    public RoutingState Router { get; } = new();
}
