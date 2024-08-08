using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using Yona.Core.App;
using Yona.Core.Projects.Models;

namespace Yona.Core.Projects;

public class TemplateRepository
{
    private readonly ILogger? log;
    private readonly string templatesDir;
    private readonly ObservableCollection<ProjectBundle> _templates = [];

    public TemplateRepository(AppService app, ILogger log)
    {
        this.log = log;

        this.templatesDir = Path.Join(app.BaseDir, "templates");
        Directory.CreateDirectory(this.templatesDir);

        LoadTemplates();
    }

    public IReadOnlyList<ProjectBundle> Items => this._templates;

    private void LoadTemplates()
    {
        foreach (var dir in Directory.EnumerateDirectories(this.templatesDir))
        {
            var templateFile = Path.Join(dir, "project.yaml");
            if (File.Exists(templateFile))
            {
                try
                {
                    var template = new ProjectBundle(templateFile);
                    this._templates.Add(template);
                }
                catch (Exception ex)
                {
                    this.log?.LogError(ex, "Failed to load project template file.\nFile: {file}", templateFile);
                }
            }
        }
    }
}
