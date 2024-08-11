using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using System.Reactive.Linq;
using Yona.Core.Projects;
using Yona.Core.Projects.Models;
using Yona.Core.ViewModels.CreateProject;

namespace Yona.Core.ViewModels.Dashboard.Home;

public partial class HomeViewModel : ViewModelBase, IActivatableViewModel
{
    private readonly ProjectRepository projects;
    private readonly ProjectServices services;
    private readonly TemplateRepository templates;
    private readonly ILogger log;

    public HomeViewModel(
        ProjectRepository projects,
        ProjectServices services,
        TemplateRepository templates,
        ILogger log)
    {
        this.log = log;
        this.projects = projects;
        this.services = services;
        this.templates = templates;
    }

    public List<ProjectBundle> RecentProjects => this.projects.Items.Take(10).ToList();

    public IReadOnlyList<ProjectBundle> Templates => this.templates.Items;

    public Interaction<CreateProjectViewModel, bool> ShowCreateProject { get; } = new();

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

            var createProjectVm = new CreateProjectViewModel(newProject, this.services);

            var confirmed = await this.ShowCreateProject.Handle(createProjectVm);
            if (confirmed)
            {
                this.projects.Add(newProject);
            }
        }
        catch (Exception ex)
        {
            this.log.LogError(ex, "Failed to create new project.");
        }
    }
}
