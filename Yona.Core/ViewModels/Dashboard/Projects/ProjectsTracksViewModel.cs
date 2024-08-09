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
    private readonly IRelayCommand closePanelCommand;
    private readonly RelayCommand saveProjectCommand;
    private readonly TrackPanelFactory trackPanel;
    private readonly ProjectBuilder builder;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TrackPanel))]
    private AudioTrack? _selectedTrack;

    public ProjectTracksViewModel(IScreen host, ProjectBundle project, TrackPanelFactory trackPanel, ProjectBuilder builder)
    {
        this.HostScreen = host;
        this.trackPanel = trackPanel;
        this.builder = builder;
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
