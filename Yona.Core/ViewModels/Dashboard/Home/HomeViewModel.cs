using CommunityToolkit.Mvvm.Input;
using DynamicData;
using DynamicData.Binding;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Yona.Core.Projects;
using Yona.Core.Projects.Models;
using Yona.Core.ViewModels.CreateProject;
using Yona.Core.ViewModels.Dashboard.Projects;

namespace Yona.Core.ViewModels.Dashboard.Home;

public partial class HomeViewModel : ViewModelBase, IActivatableViewModel
{
    private readonly ProjectsRouter projectsRouter;
    private readonly ProjectRepository projects;
    private readonly ProjectServices services;
    private readonly TemplateRepository templates;
    private readonly ILogger log;

    private readonly ObservableCollection<string> activeTags = [];
    private readonly ObservableAsPropertyHelper<IEnumerable<Tag>> _tags;
    private readonly ObservableAsPropertyHelper<IEnumerable<ProjectBundle>> _filteredTemplates;

    public IEnumerable<Tag> Tags => _tags.Value;

    public IEnumerable<ProjectBundle> Templates => _filteredTemplates.Value;

    public HomeViewModel(
        ProjectsViewModel projectsVm,
        ProjectRepository projects,
        ProjectServices services,
        TemplateRepository templates,
        ILogger log)
    {
        this.log = log;
        this.projectsRouter = (ProjectsRouter)projectsVm.Router;
        this.projects = projects;
        this.services = services;
        this.templates = templates;

        var projectsObs = this.projects.Items.WhenAnyPropertyChanged();
        var templatesObs = this.templates.Items.WhenAnyPropertyChanged();

        _filteredTemplates =
            Observable.Merge<object?>(templatesObs, this.activeTags.WhenAnyPropertyChanged())
            .Select(_ =>
            {
                if (this.activeTags.Count > 0)
                {

                    return this.templates.Items.Where(template => MatchesTags(template, this.activeTags.ToArray()));
                }

                return this.templates.Items;
            })
            .ToProperty(this, x => x.Templates, initialValue: this.templates.Items);

        _tags = templatesObs
            .Select(_ => GetAllProjectTags(this.Templates))
            .ToProperty(this, x => x.Tags, initialValue: GetAllProjectTags(this.Templates));

        this.WhenActivated((CompositeDisposable disp) =>
        {
            this.activeTags.Clear();
            projectsObs.Subscribe(_ => this.OnPropertyChanged(nameof(RecentProjects))).DisposeWith(disp);
        });
    }

    private static IEnumerable<Tag> GetAllProjectTags(IEnumerable<ProjectBundle> projects)
        => projects.SelectMany(x => x.Data.Tags).ToHashSet().Select(x => new Tag() { Name = x });

    private static bool MatchesTags(ProjectBundle project, string[] reqTags)
    {
        var numMatches = 0;
        foreach (var tag in project.Data.Tags)
        {
            if (reqTags.Contains(tag))
            {
                numMatches++;
            }
        }

        return numMatches == reqTags.Length;
    }

    public List<ProjectBundle> RecentProjects => this.projects.Items.Take(10).ToList();

    public Interaction<CreateProjectViewModel, bool> ShowCreateProject { get; } = new();

    public ViewModelActivator Activator { get; } = new();

    [RelayCommand]
    private async Task CreateProject(ProjectBundle template)
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

    [RelayCommand]
    private void ToggleTag(Tag tag)
    {
        if (tag.Enabled)
        {
            this.activeTags.Add(tag.Name);
        }
        else
        {
            this.activeTags.Remove(tag.Name);
        }
    }

    [RelayCommand]
    private void OpenProject(ProjectBundle project)  => this.projectsRouter.OpenProject(project);

    public class Tag
    {
        public bool Enabled { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
