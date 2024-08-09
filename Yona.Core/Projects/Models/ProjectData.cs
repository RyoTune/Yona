using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Yona.Core.Audio.Models;

namespace Yona.Core.Projects.Models;

public partial class ProjectData : ObservableObject
{
    [ObservableProperty]
    private string id = Guid.NewGuid().ToString();

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private int version = 1;

    /// <summary>
    /// Default encoder for new tracks.
    /// </summary>
    [ObservableProperty]
    private string defaultEncoder = "HCA";

    /// <summary>
    /// Default loop state for new tracks.
    /// </summary>
    [ObservableProperty]
    private bool defaultLoopState;

    /// <summary>
    /// Default output path for new tracks.
    /// </summary>
    [ObservableProperty]
    private string defaultOutputPath = string.Empty;

    [ObservableProperty]
    private ObservableCollection<AudioTrack> tracks = [];
}
