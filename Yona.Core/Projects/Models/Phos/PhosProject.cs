using CommunityToolkit.Mvvm.ComponentModel;
using Yona.Core.Audio.Loops;
using Yona.Core.Audio.Models;
using Yona.Core.Utils.Serializers;

namespace Yona.Core.Projects.Models.Phos;

internal static class PhosProject
{
    public static ProjectBundle Convert(string phosFile)
    {
        var phosProject = JsonFileSerializer.Instance.DeserializeFile<PhosProjectSettings>(phosFile);

        var phosTracksFile = Path.Join(Path.GetDirectoryName(phosFile)!, "audio", "audio-tracks.json");
        var phosTracks = JsonFileSerializer.Instance.DeserializeFile<PhosAudioTrack[]>(phosTracksFile);

        var newProjectFile = Path.Join(Path.GetDirectoryName(phosFile)!, "project.yaml");
        var newProject = new ProjectBundle(newProjectFile);

        newProject.Data.Id = phosProject.Id;
        newProject.Data.Name = phosProject.Name;
        newProject.Data.OutputDir = phosProject.OutputDir;
        newProject.Data.DefaultLoopState = true;
        newProject.Data.Tracks = new(phosTracks.Select(x => new AudioTrack()
        {
            Name = x.Name,
            Category = x.Category,
            Encoder = x.Encoder!,
            InputFile = x.ReplacementFile,
            OutputPath = x.OutputPath,
            Loop = new(x.Loop),
            Tags = x.Tags,
        }));

        return newProject;
    }
}

internal partial class PhosAudioTrack : ObservableObject
{
    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string? category;

    [ObservableProperty]
    private string[] tags = [];

    [ObservableProperty]
    private string? outputPath;

    [ObservableProperty]
    private string? replacementFile;

    [ObservableProperty]
    private string? encoder;

    [ObservableProperty]
    private Loop loop = new();
}

internal partial class PhosProjectSettings : ObservableObject
{
    [ObservableProperty]
    private string id = Guid.NewGuid().ToString();

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string? outputDir;
}