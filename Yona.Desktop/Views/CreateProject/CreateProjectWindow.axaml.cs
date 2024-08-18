using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Disposables;
using Yona.Core.ViewModels.CreateProject;
using Yona.Desktop.Controls;
using Yona.Desktop.Extensions;

namespace Yona.Desktop.Views.CreateProject;

public partial class CreateProjectWindow : ReactiveSukiWindow<CreateProjectViewModel>
{
    private bool createConfirmed;

    public CreateProjectWindow()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            this.ViewModel!.SelectFile.RegisterHandler(this.HandleFileSelect).DisposeWith(disposables);
            this.ViewModel!.SelectFolder.RegisterHandler(this.HandleFolderSelect).DisposeWith(disposables);
            this.ViewModel!.Close.RegisterHandler(this.HandleCloseInteraction).DisposeWith(disposables);
            this.ViewModel!.ConfirmCreate.RegisterHandler(this.HandleConfirmCreate).DisposeWith(disposables);
            this.ViewModel!.ConfirmDelete.RegisterHandler(this.HandleConfirmInteraction).DisposeWith(disposables);
        });
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        if (this.createConfirmed == false)
        {
            this.ViewModel?.CancelProject();
        }
    }

    private void HandleConfirmCreate(IInteractionContext<Unit, Unit> context)
    {
        this.createConfirmed = true;
        context.SetOutput(new());
        this.Close(true);
    }
}