using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Yona.Core.ViewModels.Convert;

namespace Yona.Desktop.Views.Convert;

public partial class ConvertView : ReactiveUserControl<ConvertViewModel>
{
    public ConvertView()
    {
        InitializeComponent();
    }
}