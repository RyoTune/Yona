using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Yona.Core.Projects;
using Yona.Core.Projects.Models;
using Yona.Core.ViewModels.TrackPanel;

namespace Yona.Core.ViewModels.Dashboard.Projects;

public partial class ProjectsViewModel : ViewModelBase
{
    private readonly IRelayCommand closePanelCommand;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TrackPanel))]
    private AudioTrack? _selectedTrack;

    public ProjectsViewModel(TemplatesRepository templates)
    {
        this.closePanelCommand = new RelayCommand(() => this.SelectedTrack = null);
        this.Tracks = templates.Items[3].Tracks;
    }

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

    public IReadOnlyList<AudioTrack> Tracks { get; set; }
}
