using Avalonia.ReactiveUI;
using Yona.Core.ViewModels.Dashboard.Projects;

namespace Yona.Desktop.Views.Dashboard.Projects;

public partial class ProjectTracksView : ReactiveUserControl<ProjectTracksViewModel>
{
    public ProjectTracksView()
    {
        InitializeComponent();
    }
}