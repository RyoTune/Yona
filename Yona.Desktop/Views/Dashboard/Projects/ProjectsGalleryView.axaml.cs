using Avalonia.ReactiveUI;
using Yona.Core.ViewModels.Dashboard.Projects;

namespace Yona.Desktop.Views.Dashboard.Projects;

public partial class ProjectsGalleryView : ReactiveUserControl<ProjectsGalleryViewModel>
{
    public ProjectsGalleryView()
    {
        InitializeComponent();
    }
}