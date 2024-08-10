using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using Yona.Core.Audio;
using Yona.Core.Projects;
using Yona.Core.Projects.Models;
using Yona.Core.ViewModels.CreateProject;

namespace Yona.Core.ViewModels.Dashboard.Home;

public partial class HomeViewModel : ViewModelBase, IActivatableViewModel
{
    private readonly ProjectRepository projects;
    private readonly TemplateRepository templates;
    private readonly EncoderRepository encoders;
    private readonly ILogger log;

    public HomeViewModel(
        ProjectRepository projects,
        TemplateRepository templates,
        EncoderRepository encoders,
        ILogger log)
    {
        this.log = log;
        this.projects = projects;
        this.templates = templates;
        this.encoders = encoders;
    }

    public List<ProjectBundle> RecentProjects => this.projects.Items.Take(10).ToList();

    public IReadOnlyList<ProjectBundle> Templates => this.templates.Items;

    public Interaction<CreateProjectViewModel, Unit> ShowCreateProject { get; } = new();

    public ViewModelActivator Activator { get; } = new();

    [RelayCommand]
    public async Task CreateProject(ProjectBundle template)
    {
        try
        {
            // TODO: Tracks are a reference collection here.
            // new projects will edit the tracks as the template's probably.
            // Have to clone first.
            var newProjectData = new ProjectData(template.Data);
            var newProject = this.projects.Create(newProjectData);

            await this.ShowCreateProject.Handle(new(newProject, this.templates, this.encoders));
        }
        catch (Exception ex)
        {
            this.log.LogError(ex, "Failed to create new project.");
        }
    }
}
