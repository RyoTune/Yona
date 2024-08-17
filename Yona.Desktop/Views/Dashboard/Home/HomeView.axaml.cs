using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Yona.Core.ViewModels.CreateProject;
using Yona.Core.ViewModels.Dashboard.Home;
using Yona.Desktop.Views.Convert;
using Yona.Desktop.Views.CreateProject;

namespace Yona.Desktop.Views.Dashboard.Home;

public partial class HomeView : ReactiveUserControl<HomeViewModel>
{
    public HomeView()
    {
        InitializeComponent();

        this.ConvertDrop.AddHandler(DragDrop.DropEvent, this.Drop);

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

    private async Task Drop(object? sender, DragEventArgs e)
    {
        if (e.Data.Contains(DataFormats.Files))
        {
            var paths = e.Data.GetFiles()?.ToArray() ?? [];
            if (paths.Length < 1)
            {
                return;
            }

            var files = new List<string>();
            foreach (var path in paths)
            {
                var localPath = path.Path.LocalPath;
                if (File.Exists(localPath))
                {
                    files.Add(localPath);
                    continue;
                }
                else if (Directory.Exists(localPath))
                {
                    var dirFiles = Directory.GetFiles(localPath, "*", SearchOption.AllDirectories);
                    files.AddRange(dirFiles);
                }
            }

            var convertVm = await this.ViewModel!.ConvertDrop.Handle(files.ToArray());
            var convertWin = new ConvertWindow() { DataContext = convertVm };
            convertWin.Show();
        }
    }
}