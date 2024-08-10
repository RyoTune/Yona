using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Yona.Core.ViewModels.CreateProject;
using Yona.Core.ViewModels.Dashboard.Projects;
using Yona.Desktop.Views.CreateProject;

namespace Yona.Desktop.Views.Dashboard.Projects;

public partial class ProjectTracksView : ReactiveUserControl<ProjectTracksViewModel>
{
    public ProjectTracksView()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            this.ViewModel!.EditProject.RegisterHandler(this.HandleEditProject).DisposeWith(disposables);
        });
    }

    private async Task HandleEditProject(IInteractionContext<CreateProjectViewModel, Unit> context)
    {
        var createProjectWindow = new CreateProjectWindow() { DataContext = context.Input };
        await createProjectWindow.ShowDialog((Window)TopLevel.GetTopLevel(this)!);
        context.SetOutput(new());
    }
}