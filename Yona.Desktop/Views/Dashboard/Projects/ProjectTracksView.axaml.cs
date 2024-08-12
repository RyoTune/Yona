using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Yona.Core.ViewModels.CreateProject;
using Yona.Core.ViewModels.CreateTrack;
using Yona.Core.ViewModels.Dashboard.Projects;
using Yona.Desktop.Views.CreateProject;
using Yona.Desktop.Views.CreateTrack;

namespace Yona.Desktop.Views.Dashboard.Projects;

public partial class ProjectTracksView : ReactiveUserControl<ProjectTracksViewModel>
{
    public ProjectTracksView()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            this.ViewModel?.EditProject.RegisterHandler(this.HandleEditProject).DisposeWith(disposables);
            this.ViewModel?.AddTrack.RegisterHandler(this.HandleAddTrack).DisposeWith(disposables);
        });
    }

    private async Task HandleAddTrack(IInteractionContext<CreateTrackViewModel, Unit> context)
    {
        var createTrackWindow = new CreateTrackWindow() { DataContext = context.Input };
        await createTrackWindow.ShowDialog((Window)TopLevel.GetTopLevel(this)!);
        context.SetOutput(new());
    }

    private async Task HandleEditProject(IInteractionContext<CreateProjectViewModel, Unit> context)
    {
        var createProjectWindow = new CreateProjectWindow() { DataContext = context.Input };
        await createProjectWindow.ShowDialog((Window)TopLevel.GetTopLevel(this)!);
        context.SetOutput(new());
    }
}