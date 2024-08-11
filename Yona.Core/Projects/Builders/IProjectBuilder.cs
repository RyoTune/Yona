using Yona.Core.Projects.Models;

namespace Yona.Core.Projects.Builders;

public interface IProjectBuilder
{
    Task Build(ProjectBundle project, IProgress<float>? progress);
}
