using ReactiveUI;
using System.Reactive.Disposables;
using Yona.Core.ViewModels.Convert;
using Yona.Desktop.Controls;
using Yona.Desktop.Extensions;

namespace Yona.Desktop.Views.Convert;

public partial class ConvertWindow : ReactiveSukiWindow<ConvertViewModel>
{
    public ConvertWindow()
    {
        InitializeComponent();

        this.WhenActivated((CompositeDisposable disp) =>
        {
            this.ViewModel?.OutputFolder.RegisterHandler(this.HandleFolderSelect).DisposeWith(disp);
        });
    }
}