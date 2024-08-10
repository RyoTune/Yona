using Avalonia.Controls;

namespace Yona.Desktop.Controls;

/// <summary>
/// Ideally, this should just be part of <see cref="Views.CreateProject.CreateProjectWindow"/>,
/// but Avalonia previewer doesn't work with <seealso cref="SukiUI.Controls.SukiWindow"/>.
/// </summary>
public partial class CreateProject : UserControl
{
    public CreateProject()
    {
        InitializeComponent();
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var window = (Window)TopLevel.GetTopLevel(this)!;
        window.Close();
    }
}