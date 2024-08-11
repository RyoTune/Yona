using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Yona.Core.Common.Dialog;
using Yona.Core.Common.Interactions;
using Yona.Core.Extensions;
using Yona.Core.Projects;
using Yona.Core.Projects.Models;

namespace Yona.Core.ViewModels.CreateProject;

public partial class CreateProjectViewModel : ViewModelBase, IActivatableViewModel
{
    private readonly ProjectServices services;

    public CreateProjectViewModel(ProjectBundle project, ProjectServices services)
    {
        this.Project = project;
        this.services = services;

        this.Templates = services.Templates;
        this.Encoders = services.Encoders;

        this.WhenActivated((CompositeDisposable disposables) =>
        {
            project.AutosaveWithChanges().DisposeWith(disposables);
        });
    }

    public ProjectBundle Project { get; }

    public bool IsEditing { get; init; }

    public string[] Encoders { get; }

    public string[] Templates { get; }

    public ViewModelActivator Activator { get; } = new();

    public FileSelectInteraction SelectFile { get; } = new();

    public FolderSelectInteraction SelectFolder { get; } = new();

    public CloseInteraction Close { get; } = new();

    public CloseInteraction ConfirmCreate { get; } = new();

    public ConfirmInteraction ConfirmDelete { get; } = new();

    [RelayCommand]
    private async Task SelectOutputFolder()
    {
        var folders = await this.SelectFolder.Handle(new() { Title = "Select Output Folder" });
        if (folders.Length > 0)
        {
            this.Project.Data.OutputDir = folders[0];
        }
    }

    [RelayCommand]
    private async Task SelectProjectIcon()
    {
        var fileOptions = new FileSelectOptions()
        {
            Title = "Select Project Icon",
            Filter = "Image Files (*.png)|*.png",
        };

        var files = await this.SelectFile.Handle(fileOptions);
        if (files.Length > 0)
        {
            try
            {
                this.Project.UpdateIcon(files[0]);
            }
            catch (Exception)
            {
                // TODO: Log error.
            }
        }
    }

    [RelayCommand]
    private async Task Confirm()
    {
        if (this.IsEditing == false)
        {
            await this.ConfirmCreate.Handle(new());
        }
        else
        {
            await this.Close.Handle(new());
        }
    }

    [RelayCommand]
    private async Task Cancel()
    {
        try
        {
            this.services.DeleteProject(this.Project);
            await this.Close.Handle(new());
        }
        catch (Exception)
        {
            // TODO: Log error.
        }
    }

    [RelayCommand]
    private async Task Delete()
    {
        try
        {
            var confirmed = await this.ConfirmDelete.Handle(new()
            {
                Title = $"Delete Project?",
                Description = $"Are you sure you want to delete: {this.Project.Data.Name}",
            });

            if (confirmed)
            {
                this.services.DeleteProject(this.Project);
                await this.Close.Handle(new());
            }
        }
        catch (Exception)
        {
            // TODO: Log error.
        }
    }

    [RelayCommand]
    private void Reset() => this.services.ResetProject(this.Project);
}
