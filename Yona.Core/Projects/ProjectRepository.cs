﻿using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using Yona.Core.App;
using Yona.Core.Projects.Models;

namespace Yona.Core.Projects;

public class ProjectRepository
{
    private readonly ILogger log;
    private readonly string projectsDir;
    private readonly ObservableCollection<ProjectBundle> projects = [];

    public ProjectRepository(AppService app, ILogger log)
    {
        this.log = log;
        this.projectsDir = Path.Join(app.BaseDir, "projects");
        Directory.CreateDirectory(this.projectsDir);

        this.LoadProjects();
    }

    public ReadOnlyObservableCollection<ProjectBundle> Items => new(this.projects);

    public void Delete(ProjectBundle project)
    {
        var projectDir = Path.GetDirectoryName(project.ProjectFile)!;
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
        return newProject;
    }

    public void Add(ProjectBundle project) => this.projects.Add(project);

    public ProjectBundle Get(string id) => this.projects.First(x => x.Data.Id == id);

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
                    this.projects.Add(project);
                }
                catch (Exception ex)
                {
                    this.log.LogError(ex, "Failed to load project.\nFile: {file}", projectFile);
                }
            }
        }
    }
}
