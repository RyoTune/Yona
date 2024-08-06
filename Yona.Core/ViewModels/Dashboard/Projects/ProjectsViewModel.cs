using Yona.Core.Projects;
using Yona.Core.Projects.Models;

namespace Yona.Core.ViewModels.Dashboard.Projects;

public class ProjectsViewModel : ViewModelBase
{
    public ProjectsViewModel(TemplatesRegistry templates)
    {
        Tracks = templates.Templates[3].Tracks;
    }

    public List<AudioTrack> Tracks { get; set; }
}
