using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Yona.Desktop.Controls;

public class ShellWindow<TViewModel> : ReactiveSukiWindow<TViewModel>
    where TViewModel : class
{
    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        if (this.Content is ContentControl content)
        {
            var size = content.DesiredSize;
            this.Width = size.Width;
            this.Height = size.Height + 40;
            this.CanResize = false;
        }
    }
}
