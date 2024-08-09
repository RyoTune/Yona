using Yona.Core.Common;
using Yona.Core.Utils.Serializers;

namespace Yona.Core.Projects.Models;

public class ProjectBundle : SavableFile<ProjectData>
{
    public ProjectBundle(string filePath) : base(filePath, YamlFileSerializer.Instance)
    {
        this.ProjectFile = filePath;
        this.ProjectDir = Path.GetDirectoryName(filePath)!;
        this.IconFile = Path.ChangeExtension(filePath, ".png");
        this.BuildDir = Path.Join(this.ProjectDir, "build");

        Directory.CreateDirectory(this.BuildDir);
    }

    public string ProjectFile { get; }

    public string ProjectDir { get; }

    public string IconFile { get; }

    public string BuildDir { get; }
}
