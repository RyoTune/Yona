using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using System.Reactive;
using Yona.Core.Audio.Models;
using Yona.Core.Projects.Models;
using Yona.Core.ViewModels.TrackPanel;

namespace Yona.Core.ViewModels.Dashboard.Projects;

public partial class ProjectTracksViewModel : ViewModelBase, IRoutableViewModel
{
    private readonly IRelayCommand closePanelCommand;
    private readonly RelayCommand saveProjectCommand;
    private readonly TrackPanelFactory trackPanel;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TrackPanel))]
    private AudioTrack? _selectedTrack;

    public ProjectTracksViewModel(IScreen host, ProjectBundle project, TrackPanelFactory trackPanel)
    {
        this.trackPanel = trackPanel;
        this.HostScreen = host;
        this.UrlPathSegment = $"{project.Data.Id}/tracks";

        this.closePanelCommand = new RelayCommand(() => this.SelectedTrack = null);
        this.saveProjectCommand = new RelayCommand(project.Save);
        this.Project = project;

    }

    public ProjectBundle Project { get; }

    public IScreen HostScreen { get; }

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
}
