using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Yona.Core.Audio.Models;
using Yona.Core.Projects;
using Yona.Core.Projects.Models;
using Yona.Core.ViewModels.CreateProject;
using Yona.Core.ViewModels.TrackPanel;
using FuzzySharp;

namespace Yona.Core.ViewModels.Dashboard.Projects;

public partial class ProjectTracksViewModel : ViewModelBase, IRoutableViewModel, IActivatableViewModel
{
    private readonly ProjectsRouter router;
    private readonly ProjectServices services;
    private readonly IRelayCommand closePanelCommand;
    private readonly RelayCommand saveProjectCommand;
    private readonly TrackPanelFactory trackPanel;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TrackPanel))]
    private AudioTrack? _selectedTrack;

    private string _searchText = string.Empty;

    private ObservableAsPropertyHelper<IEnumerable<AudioTrack>> _filteredTracks;

    public IEnumerable<AudioTrack> FilteredTracks => this._filteredTracks.Value;

    public ProjectTracksViewModel(ProjectsRouter router, ProjectBundle project, ProjectServices services, TrackPanelFactory trackPanel)
    {
        this.router = router;
        this.services = services;
        this.trackPanel = trackPanel;
        this.UrlPathSegment = $"{project.Data.Id}/tracks";

        this.closePanelCommand = new RelayCommand(() => this.SelectedTrack = null);
        this.saveProjectCommand = new RelayCommand(project.Save);
        this.Project = project;

        var searchObs = this.WhenAnyValue(x => x.SearchText)
                        .Throttle(TimeSpan.FromMilliseconds(200));

        var tracksObs = this.WhenAnyValue(x => x.Project.Data.Tracks).Select(x => x.ToObservableChangeSet().SkipInitial().AutoRefresh());

        this._filteredTracks = Observable.Merge<object?>(searchObs, tracksObs)
        .Select(_ =>
        {
            if (string.IsNullOrEmpty(this.SearchText))
            {
                return this.Project.Data.Tracks;
            }
            else
            {
                var sorted = this.Project.Data.Tracks.Where(x => Fuzz.Ratio(this.SearchText.ToLower(), x.Name.ToLower()) > Math.Min(this.SearchText.Length * 5, 90)).ToArray();
                return (IEnumerable<AudioTrack>)sorted;
            }
        })
        .ToProperty(this, x => x.FilteredTracks);

        this.WhenActivated((CompositeDisposable disp) =>
        {
            this._filteredTracks.DisposeWith(disp);
        });
    }

    public string SearchText
    {
        get => this._searchText;
        set => this.RaiseAndSetIfChanged(ref this._searchText, value);
    }

    public IScreen HostScreen => this.router.HostScreen;

    public ProjectBundle Project { get; }

    public string? UrlPathSegment { get; }

    public ReactiveCommand<Unit, IRoutableViewModel> GoBackCommand => this.HostScreen.Router.NavigateBack;

    public Interaction<CreateProjectViewModel, Unit> EditProject { get; } = new();

    public TrackPanelViewModel? TrackPanel
    {
        get
        {
            if (this.SelectedTrack != null)
            {
                return this.trackPanel.Create(this.SelectedTrack, this.saveProjectCommand, this.closePanelCommand);
            }

            return null;
        }
    }

    public ViewModelActivator Activator { get; } = new();

    [RelayCommand]
    private async Task Build()
    {
        try
        {
            await this.services.BuildProject(this.Project);
        }
        catch (Exception)
        {
        }
    }

    [RelayCommand]
    private async Task OpenEditProject()
    {
        await this.EditProject.Handle(new(this.Project, this.services) { IsEditing = true });
    }
}
