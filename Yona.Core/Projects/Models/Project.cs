using CommunityToolkit.Mvvm.ComponentModel;

namespace Yona.Core.Projects.Models;

public partial class Project : ObservableObject
{
    [ObservableProperty]
    private string id = Guid.NewGuid().ToString();

    [ObservableProperty]
    private string name = string.Empty;
}
