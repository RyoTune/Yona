using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using Yona.Core.App;
using Yona.Core.Projects.Models;

namespace Yona.Core.Projects;

public class ProjectRepository
{
    private readonly ILogger log;
    private readonly string projectsDir;
    private readonly ObservableCollection<ProjectBundle> _projects = [];

    public ProjectRepository(AppService app, ILogger log)
    {
        this.log = log;
        this.projectsDir = Path.Join(app.BaseDir, "projects");
        Directory.CreateDirectory(this.projectsDir);

        this.LoadProjects();
    }

    public IReadOnlyList<ProjectBundle> Items => this._projects;

    public void Delete(ProjectBundle entity)
    {
        var projectDir = Path.GetDirectoryName(entity.ProjectFile)!;
        if (Directory.Exists(projectDir))
        {
            Directory.Delete(projectDir, true);
        }

        this._projects.Remove(entity);
    }

    public ProjectBundle Create(ProjectData project)
    {
        var newProjectFile = Path.Join(this.projectsDir, project.Id, "project.yaml");
        Directory.CreateDirectory(Path.GetDirectoryName(newProjectFile)!);

        var newProject = new ProjectBundle(newProjectFile)
        {
            Data = project,
        };

        newProject.Save();
        this._projects.Add(newProject);

        return newProject;
    }

    public ProjectBundle Get(string id) => this._projects.First(x => x.Data.Id == id);

    private void LoadProjects()
    {
        foreach (var dir in Directory.EnumerateDirectories(this.projectsDir))
        {
            var projectFile = Path.Join(dir, "project.yaml");
            if (File.Exists(projectFile))
            {
                try
                {
                    var project = new ProjectBundle(projectFile);
                    this._projects.Add(project);
                }
                catch (Exception ex)
                {
                    this.log.LogError(ex, "Failed to load project.\nFile: {file}", projectFile);
                }
            }
        }
    }
}
