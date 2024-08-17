using Avalonia;
using Avalonia.Controls;
using Yona.Core.Audio.Models;

namespace Yona.Desktop.Controls;

public partial class LoopInput : UserControl
{
    public static readonly StyledProperty<bool> InputEnabledProperty =
        AvaloniaProperty.Register<LoopInput, bool>(nameof(InputEnabled));

    public static readonly StyledProperty<ObservableLoop> LoopProperty =
        AvaloniaProperty.Register<LoopInput, ObservableLoop>(nameof(LoopProperty));

    public LoopInput()
    {
        InitializeComponent();
    }

    public bool InputEnabled
    {
        get => this.GetValue(InputEnabledProperty);
        set => this.SetValue(InputEnabledProperty, value);
    }

    public ObservableLoop Loop
    {
        get => this.GetValue(LoopProperty);
        set => this.SetValue(LoopProperty, value);
    }
}