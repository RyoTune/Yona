using Yona.Core.Common;
using Yona.Core.Common.Serializers;

namespace Yona.Core.Projects.Models;

public class ProjectBundle(string filePath)
    : SavableFile<ProjectData>(filePath, YamlFileSerializer.Instance)
{
    public string File { get; } = filePath;

    public string IconFile { get; } = Path.ChangeExtension(filePath, ".png");
}
