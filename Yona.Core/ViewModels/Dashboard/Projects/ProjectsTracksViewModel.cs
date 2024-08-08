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

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TrackPanel))]
    private AudioTrack? _selectedTrack;

    public ProjectTracksViewModel(IScreen host, ProjectBundle project)
    {
        this.HostScreen = host;
        this.UrlPathSegment = $"{project.Data.Id}/tracks";

        this.closePanelCommand = new RelayCommand(() => this.SelectedTrack = null);
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
                return new(this.SelectedTrack, this.closePanelCommand);
            }

            return null;
        }
    }
}
