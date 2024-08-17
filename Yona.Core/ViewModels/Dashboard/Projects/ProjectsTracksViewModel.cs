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
using Yona.Core.ViewModels.CreateTrack;
using FuzzySharp.SimilarityRatio.Scorer.StrategySensitive;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Yona.Core.ViewModels.Dashboard.Projects;

public partial class ProjectTracksViewModel : ViewModelBase, IRoutableViewModel, IActivatableViewModel
{
    public const int MIN_BUILD_TIME_MS = 1500;

    private readonly ILogger<ProjectTracksViewModel> log;
    private readonly ProjectsRouter router;
    private readonly ProjectServices services;
    private readonly IRelayCommand closePanelCommand;
    private readonly TrackPanelFactory trackPanel;
    private readonly PartialRatioScorer scorer = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TrackPanel))]
    private AudioTrack? _selectedTrack;

    private string _searchText = string.Empty;

    private ObservableAsPropertyHelper<IEnumerable<AudioTrack>> _filteredTracks;

    public IEnumerable<AudioTrack> FilteredTracks => this._filteredTracks.Value;

    public ProjectTracksViewModel(
        ProjectsRouter router,
        ProjectBundle project,
        ProjectServices services,
        TrackPanelFactory trackPanel,
        ILogger<ProjectTracksViewModel> log)
    {
        this.log = log;
        this.router = router;
        this.services = services;
        this.trackPanel = trackPanel;
        this.UrlPathSegment = $"{project.Data.Id}/tracks";

        this.closePanelCommand = new RelayCommand(() => this.SelectedTrack = null);
        this.Project = project;

        var searchObs = this.WhenAnyValue(x => x.SearchText)
                        .Throttle(TimeSpan.FromMilliseconds(200));

        var tracksObs = this.WhenAnyValue(x => x.Project.Data.Tracks)
            .Select(x => x.ToObservableChangeSet().SkipInitial().AutoRefresh());

        this._filteredTracks = Observable.Merge<object?>(searchObs, tracksObs)
            .Select(_ =>
            {
                if (string.IsNullOrEmpty(this.SearchText))
                {
                    return this.Project.Data.Tracks;
                }
                else
                {
                    try
                    {
                        var processed = FuzzySharp.Process.ExtractTop(new AudioTrack() { Name = this.SearchText }, this.Project.Data.Tracks, x => x.Name.ToLower(), scorer, cutoff: Math.Min(this.SearchText.Length * 5, 90));
                        return processed.Select(x => x.Value);
                    }
                    catch (Exception ex)
                    {
                        log.LogError(ex, "Failed to filter audio tracks.");
                        return [];
                    }
                }
            })
            .ObserveOn(RxApp.MainThreadScheduler)
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

    public Interaction<CreateTrackViewModel, Unit> AddTrack { get; } = new();

    public TrackPanelViewModel? TrackPanel
    {
        get
        {
            if (this.SelectedTrack != null)
            {
                return this.trackPanel.Create(this.SelectedTrack, this.Project, this.closePanelCommand);
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
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            await this.services.BuildProject(this.Project);
            stopwatch.Stop();

#if RELEASE
            if (stopwatch.ElapsedMilliseconds < MIN_BUILD_TIME_MS)
            {
                await Task.Delay((int)(MIN_BUILD_TIME_MS - stopwatch.ElapsedMilliseconds));
            }
#endif

            this.log.LogInformation("Project built successfully in {time}ms!", stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            this.log.LogError(ex, "Failed to build project.");
        }
    }

    [RelayCommand]
    private async Task OpenEditProject()
    {
        await this.EditProject.Handle(new(this.Project, this.services) { IsEditing = true });
    }

    [RelayCommand]
    private async Task OpenAddTrack()
    {
        await this.AddTrack.Handle(new(this.Project, this.services.Encoders));
    }
}
