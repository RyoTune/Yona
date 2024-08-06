using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using Yona.Core.App;
using Yona.Core.Common.Serializers;
using Yona.Core.Projects.Models;

namespace Yona.Core.Projects;

public class TemplatesRegistry
{
    private readonly ILogger? log;
    private readonly string templatesDir;

    public TemplatesRegistry(AppService app, ILogger? log = null)
    {
        this.log = log;
        this.templatesDir = Directory.CreateDirectory(Path.Join(app.BaseDir, "templates")).FullName;
        LoadTemplates();
    }

    public ObservableCollection<ProjectTemplate> Templates { get; } = [];

    private void LoadTemplates()
    {
        foreach (var dir in Directory.EnumerateDirectories(this.templatesDir))
        {
            var tracksFile = Path.Join(dir, "tracks.yaml");
            if (File.Exists(tracksFile))
            {
                try
                {
                    var tracks = YamlFileSerializer.Instance.DeserializeFile<ProjectTemplate>(tracksFile);
                    this.Templates.Add(tracks);
                }
                catch (Exception ex)
                {
                    this.log?.LogError(ex, "Failed to add project template.");
                }
            }
        }
    }
}
