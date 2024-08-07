using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Yona.Core.Projects.Models;

public partial class Project : ObservableObject
{
    [ObservableProperty]
    private string id = Guid.NewGuid().ToString();

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private ObservableCollection<AudioTrack> tracks = [];
}
