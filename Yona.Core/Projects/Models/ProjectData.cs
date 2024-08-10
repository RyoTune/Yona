using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Yona.Core.Audio.Models;

namespace Yona.Core.Projects.Models;

public partial class ProjectData : ObservableObject
{
    private string? _outputDir;

    public ProjectData()
    {
    }

    /// <summary>
    /// Create new project data that copies from an existing
    /// project as the template.
    /// </summary>
    /// <param name="templateProject">Existing project data.</param>
    public ProjectData(ProjectData templateProject)
    {
        this.Name = templateProject.Name;
        this.Template = templateProject.Name;
        this.DefaultEncoder = templateProject.DefaultEncoder;
        this.DefaultLoopState = templateProject.DefaultLoopState;
        this.DefaultOutputPath = templateProject.DefaultOutputPath;
        this.Tracks = templateProject.Tracks;
        this.PostBuild = templateProject.postBuild;
        this.UseFastBuild = templateProject.UseFastBuild;
    }

    [ObservableProperty]
    private string id = Guid.NewGuid().ToString();

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string? template;

    public string? OutputDir
    {
        get => this._outputDir;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                this.SetProperty(ref this._outputDir, null);
            }
            else
            {
                this.SetProperty(ref this._outputDir, value);
            }
        }
    }

    [ObservableProperty]
    private string? postBuild;

    [ObservableProperty]
    private bool useFastBuild = true;

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
