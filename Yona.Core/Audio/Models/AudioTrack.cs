using CommunityToolkit.Mvvm.ComponentModel;

namespace Yona.Core.Audio.Models;

public partial class AudioTrack : ObservableObject
{
    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private bool enabled = true;

    [ObservableProperty]
    private string? category;

    [ObservableProperty]
    private string[] tags = [];

    [ObservableProperty]
    private string? outputPath;

    [ObservableProperty]
    private string? inputFile;

    [ObservableProperty]
    private string encoder = "HCA";

    [ObservableProperty]
    private ObservableLoop loop = new();
}