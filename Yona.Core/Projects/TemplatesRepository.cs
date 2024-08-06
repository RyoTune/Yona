using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using Yona.Core.App;
using Yona.Core.Common.Interfaces;
using Yona.Core.Common.Serializers;
using Yona.Core.Projects.Models;

namespace Yona.Core.Projects;

public class TemplatesRepository : IRepository<ProjectTemplate, string>
{
    private readonly ILogger? log;
    private readonly string templatesDir;
    private readonly ObservableCollection<ProjectTemplate> _templates = new();

    private readonly Dictionary<string, string> templateFiles = [];

    public TemplatesRepository(AppService app, ILogger? log = null)
    {
        this.log = log;
        this.templatesDir = Directory.CreateDirectory(Path.Join(app.BaseDir, "templates")).FullName;
        LoadTemplates();
    }

    public IReadOnlyList<ProjectTemplate> Items => this._templates;

    public void Add(ProjectTemplate entity) => this._templates.Add(entity);

    public void Delete(ProjectTemplate entity) => this._templates.Remove(entity);

    public ProjectTemplate Get(string id) => this._templates.First(x => x.Id == id);

    public void Update(ProjectTemplate entity)
    {
        if (this.templateFiles.TryGetValue(entity.Id, out var templateFile))
        {
            YamlFileSerializer.Instance.SerializeFile(templateFile, entity);
        }

        throw new Exception($"Unknown project template: {entity.Name}\nID: {entity.Id}");
    }

    private void LoadTemplates()
    {
        foreach (var dir in Directory.EnumerateDirectories(this.templatesDir))
        {
            var templateFile = Path.Join(dir, "project.yaml");
            if (File.Exists(templateFile))
            {
                try
                {
                    var template = YamlFileSerializer.Instance.DeserializeFile<ProjectTemplate>(templateFile);
                    this._templates.Add(template);
                    this.templateFiles[template.Id] = templateFile;
                }
                catch (Exception ex)
                {
                    this.log?.LogError(ex, "Failed to load project template file.\nFile: {file}", templateFile);
                }
            }
        }
    }
}
