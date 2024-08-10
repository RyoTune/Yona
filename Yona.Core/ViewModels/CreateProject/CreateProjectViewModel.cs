using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Yona.Core.Audio;
using Yona.Core.Common.Dialog;
using Yona.Core.Extensions;
using Yona.Core.Projects;
using Yona.Core.Projects.Models;

namespace Yona.Core.ViewModels.CreateProject;

public partial class CreateProjectViewModel : ViewModelBase, IActivatableViewModel
{
    public CreateProjectViewModel(ProjectBundle project, TemplateRepository templates, EncoderRepository encoders)
    {
        this.Project = project;
        this.Templates = templates.AvailableTemplates;
        this.Encoders = encoders.AvailableEncoders;

        this.WhenActivated((CompositeDisposable disposables) =>
        {
            project.AutosaveWithChanges().DisposeWith(disposables);
        });
    }

    public ProjectBundle Project { get; }

    public string[] Encoders { get; }

    public string[] Templates { get; }

    public ViewModelActivator Activator { get; } = new();

    public FileSelectInteraction SelectFile { get; } = new();

    public FolderSelectInteraction SelectFolder { get; } = new();

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
}
