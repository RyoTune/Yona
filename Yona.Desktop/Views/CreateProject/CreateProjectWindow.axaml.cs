using ReactiveUI;
using System.Reactive.Disposables;
using Yona.Core.ViewModels.CreateProject;
using Yona.Desktop.Controls;
using Yona.Desktop.Extensions;

namespace Yona.Desktop.Views.CreateProject;

public partial class CreateProjectWindow : ReactiveSukiWindow<CreateProjectViewModel>
{
    public CreateProjectWindow()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            this.ViewModel!.SelectFile.RegisterHandler(this.HandleFileSelect).DisposeWith(disposables);
            this.ViewModel!.SelectFolder.RegisterHandler(this.HandleFolderSelect).DisposeWith(disposables);
        });
    }
}