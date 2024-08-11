using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using System.Reactive.Linq;
using Yona.Core.Projects;
using Yona.Core.Projects.Models;

namespace Yona.Core.ViewModels.Dashboard.Projects;

public class ProjectsRouter : RoutingState
{
    private readonly ProjectTracksFactory projectTracks;
    private readonly ProjectsGalleryViewModel projectsGallery;

    private ProjectBundle? currentProject;

    public ProjectsRouter(IScreen host, ProjectRepository projects, ProjectTracksFactory projectTracks)
    {
        this.projectTracks = projectTracks;
        this.HostScreen = host;
        this.projectsGallery = new ProjectsGalleryViewModel(this, projects);
        this.Navigate.Execute(this.projectsGallery);

        var projectsObs = projects.Items.ToObservableChangeSet();

        projectsObs.OnItemRemoved(item =>
        {
            if (this.currentProject == item)
            {
                this.currentProject = null;
                this.NavigateBack.Execute();
            }
        }).Subscribe();

        if (projects.Items.Count > 0)
        {
            projectsObs.SkipInitial().OnItemAdded(this.OpenProject).Subscribe();
        }
        else
        {
            projectsObs.OnItemAdded(this.OpenProject).Subscribe();
        }
    }

    public IScreen HostScreen { get; }

    public void OpenProject(ProjectBundle project)
    {
        if (this.GetCurrentViewModel() is ProjectTracksViewModel)
        {
            using var wait = this.DelayChangeNotifications();
            this.NavigateAndReset.Execute(this.projectsGallery);
        }

        this.Navigate.Execute(this.projectTracks.Create(this, project));
        this.currentProject = project;
    }
}
