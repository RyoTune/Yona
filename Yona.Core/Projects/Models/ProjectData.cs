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
    private ObservableCollection<AudioTrack> tracks = [];
}
