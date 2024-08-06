using CommunityToolkit.Mvvm.ComponentModel;

namespace Yona.Core.Projects.Models;

public partial class ProjectTemplate : ObservableObject
{
    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private List<AudioTrack> tracks = [];
}
