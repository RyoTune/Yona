using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using System.Reactive;
using Yona.Core.Audio.Models;
using Yona.Core.Projects;
using Yona.Core.Projects.Models;
using Yona.Core.ViewModels.TrackPanel;

namespace Yona.Core.ViewModels.Dashboard.Projects;

public partial class ProjectTracksViewModel : ViewModelBase, IRoutableViewModel
{
    private readonly ProjectsRouter router;
    private readonly IRelayCommand closePanelCommand;
    private readonly RelayCommand saveProjectCommand;
    private readonly ProjectBuilder builder;
    private readonly TrackPanelFactory trackPanel;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TrackPanel))]
    private AudioTrack? _selectedTrack;

    public ProjectTracksViewModel(ProjectsRouter router, ProjectBundle project, ProjectBuilder builder, TrackPanelFactory trackPanel)
    {
        this.router = router;
        this.builder = builder;
        this.trackPanel = trackPanel;
        this.UrlPathSegment = $"{project.Data.Id}/tracks";

        this.closePanelCommand = new RelayCommand(() => this.SelectedTrack = null);
        this.saveProjectCommand = new RelayCommand(project.Save);
        this.Project = project;

    }

    public IScreen HostScreen => this.router.HostScreen;

    public ProjectBundle Project { get; }

    public string? UrlPathSegment { get; }

    public ReactiveCommand<Unit, IRoutableViewModel> GoBackCommand => this.HostScreen.Router.NavigateBack;

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

    [RelayCommand]
    private async Task Build()
    {
        try
        {
            await this.builder.Build(this.Project);
        }
        catch (Exception)
        {
        }
    }
}
