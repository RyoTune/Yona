using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using Yona.Core.App;
using Yona.Core.Projects.Models;
using Yona.Core.Projects.Models.Phos;

namespace Yona.Core.Projects;

public class ProjectRepository
{
    private readonly ILogger log;
    private readonly string projectsDir;
    private readonly ObservableCollection<ProjectBundle> projects = [];

    public ProjectRepository(AppService app, ILogger<ProjectRepository> log)
    {
        this.log = log;
        this.projectsDir = Path.Join(app.BaseDir, "projects");
        Directory.CreateDirectory(this.projectsDir);

        this.LoadProjects();
    }

    public ReadOnlyObservableCollection<ProjectBundle> Items => new(this.projects);

    public void Delete(ProjectBundle project)
    {
        var projectDir = Path.GetDirectoryName(project.FilePath)!;
        if (Directory.Exists(projectDir))
        {
            Directory.Delete(projectDir, true);
        }

        this.projects.Remove(project);
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
        newProject.Reload();

        return newProject;
    }

    public void Add(ProjectBundle project) => this.projects.Add(project);

    public ProjectBundle Get(string id) => this.projects.First(x => x.Data.Id == id);

    private void LoadProjects()
    {
        foreach (var dir in Directory.EnumerateDirectories(this.projectsDir))
        {
            var projectFile = Path.Join(dir, "project.yaml");
            var phosProjectFile = Path.Join(dir, "project.phos");

            if (File.Exists(projectFile))
            {
                try
                {
                    var project = new ProjectBundle(projectFile);
                    this.projects.Add(project);
                }
                catch (Exception ex)
                {
                    this.log.LogError(ex, "Failed to load project.\nFile: {file}", projectFile);
                }
            }
            else if (File.Exists(phosProjectFile))
            {
                try
                {
                    var project = PhosProject.Convert(phosProjectFile);
                    this.projects.Add(project);
                }
                catch (Exception ex)
                {
                    this.log.LogError(ex, "Failed to convert Phos project.\nFile: {file}", phosProjectFile);
                }
            }
        }
    }
}
