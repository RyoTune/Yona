using Yona.Core.Projects.Models;

namespace Yona.Core.Projects.Builders;

public class ProjectBuilder(StandardProjectBuilder standard, FastProjectBuilder fast) : IProjectBuilder
{
    private readonly StandardProjectBuilder standard = standard;
    private readonly FastProjectBuilder fast = fast;

    public Task Build(ProjectBundle project, IProgress<float>? progress)
    {
        if (project.Data.UseFastBuild)
        {
            return this.fast.Build(project, progress);
        }
        else
        {
            FastProjectBuilder.ResetBuildFile(project);
            return this.standard.Build(project, progress);
        }
    }
}
