using Avalonia;
using Avalonia.Controls;
using System.Collections.Generic;
using System.Windows.Input;

namespace Yona.Desktop.Controls;

public partial class ProjectsGallery : UserControl
{
    public static readonly StyledProperty<IEnumerable<object>?> ProjectsSourceProperty =
        AvaloniaProperty.Register<ProjectsGallery, IEnumerable<object>?>(nameof(ProjectsSource));

    public static readonly StyledProperty<ICommand?> CommandProperty =
        AvaloniaProperty.Register<ProjectsGallery, ICommand?>(nameof(Command));

    public ProjectsGallery()
    {
        InitializeComponent();
    }

    public IEnumerable<object>? ProjectsSource
    {
        get => this.GetValue(ProjectsSourceProperty);
        set => this.SetValue(ProjectsSourceProperty, value);
    }

    public ICommand? Command
    {
        get => this.GetValue(CommandProperty);
        set => this.SetValue(CommandProperty, value);
    }
}