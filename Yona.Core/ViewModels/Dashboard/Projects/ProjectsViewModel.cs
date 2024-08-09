using ReactiveUI;

namespace Yona.Core.ViewModels.Dashboard.Projects;

public partial class ProjectsViewModel : ViewModelBase, IActivatableViewModel, IScreen
{
    public ProjectsViewModel(ProjectsRouterFactory router)
    {
        this.Router = router.Create(this);
    }

    public ViewModelActivator Activator { get; } = new();

    public RoutingState Router { get; }
}
