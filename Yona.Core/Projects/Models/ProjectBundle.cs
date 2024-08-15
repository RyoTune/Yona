using CommunityToolkit.Mvvm.ComponentModel;
using Yona.Core.Common;
using Yona.Core.Utils.Serializers;

namespace Yona.Core.Projects.Models;

[INotifyPropertyChanged]
public partial class ProjectBundle : SavableFile<ProjectData>
{
    private readonly string projectBuildDir;

    public ProjectBundle(string filePath) : base(filePath, YamlFileSerializer.Instance)
    {
        this.ProjectDir = Path.GetDirectoryName(filePath)!;
        this.IconFile = Path.ChangeExtension(filePath, ".png");
        this.projectBuildDir = Path.Join(this.ProjectDir, "build");
        Directory.CreateDirectory(this.projectBuildDir);

        this.Data.Tracks = new(this.Data.Tracks.OrderBy(x => x.Name));
        this.Save();
    }

    public string ProjectDir { get; }

    public string IconFile { get; }

    public string BuildDir => this.Data.OutputDir ?? this.projectBuildDir;

    public void UpdateIcon(string newIconFile)
    {
        File.Copy(newIconFile, this.IconFile, true);
        this.OnPropertyChanged(nameof(this.IconFile));
    }
}
