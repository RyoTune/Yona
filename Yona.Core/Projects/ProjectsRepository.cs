using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using Yona.Core.App;
using Yona.Core.Common.Interfaces;
using Yona.Core.Common.Serializers;
using Yona.Core.Projects.Models;

namespace Yona.Core.Projects;

public class ProjectsRepository : IRepository<Project, string>
{
    private readonly ILogger log;
    private readonly string projectsDir;
    private readonly ObservableCollection<Project> _projects = [];
    private readonly Dictionary<string, string> projectFiles = [];

    public ProjectsRepository(AppService app, ILogger log)
    {
        this.log = log;
        this.projectsDir = Path.Join(app.BaseDir, "projects");
        Directory.CreateDirectory(this.projectsDir);

        this.LoadProjects();
    }

    public IReadOnlyList<Project> Items => this._projects;

    public void Add(Project entity)
    {
        entity.Id = Guid.NewGuid().ToString();
        var projectFile = Path.Join(this.projectsDir, entity.Id, "project.yaml");
        Directory.CreateDirectory(Path.GetDirectoryName(projectFile)!);

        YamlFileSerializer.Instance.SerializeFile(projectFile, entity);
        this.projectFiles[entity.Id] = projectFile;
        this._projects.Add(entity);
    }

    public void Delete(Project entity)
    {
        throw new NotImplementedException();
    }

    public Project Get(string id)
    {
        throw new NotImplementedException();
    }

    public void Update(Project entity)
    {
        throw new NotImplementedException();
    }

    private void LoadProjects()
    {
        foreach (var dir in Directory.EnumerateDirectories(this.projectsDir))
        {
            var projectFile = Path.Join(dir, "project.yaml");
            if (File.Exists(projectFile))
            {
                try
                {
                    var project = YamlFileSerializer.Instance.DeserializeFile<Project>(projectFile);
                    this._projects.Add(project);
                    this.projectFiles[project.Id] = projectFile;
                }
                catch (Exception ex)
                {
                    this.log.LogError(ex, "Failed to load project.\nFile: {file}", projectFile);
                }
            }
        }
    }
}
