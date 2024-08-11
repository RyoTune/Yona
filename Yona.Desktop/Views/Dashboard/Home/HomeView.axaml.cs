using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Yona.Core.ViewModels.CreateProject;
using Yona.Core.ViewModels.Dashboard.Home;
using Yona.Desktop.Views.CreateProject;

namespace Yona.Desktop.Views.Dashboard.Home;

public partial class HomeView : ReactiveUserControl<HomeViewModel>
{
    public HomeView()
    {
        InitializeComponent();

        this.WhenActivated((CompositeDisposable disposables) =>
        {
            this.ViewModel?.ShowCreateProject.RegisterHandler(this.HandleCreateProject).DisposeWith(disposables);
        });
    }

    private async Task HandleCreateProject(IInteractionContext<CreateProjectViewModel, bool> context)
    {
        var createProjectWindow = new CreateProjectWindow() { DataContext = context.Input };
        var result = await createProjectWindow.ShowDialog<bool>((Window)TopLevel.GetTopLevel(this)!);

        context.SetOutput(result);
    }
}