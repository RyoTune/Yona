using SukiUI.Controls;

namespace Yona.Desktop.Controls;

public partial class ConfirmWindow : SukiWindow
{
    public ConfirmWindow()
    {
        InitializeComponent();
    }

    private void Button_Confirm(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close(true);
    }

    private void Button_Cancel(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close(false);
    }
}