using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Reactive.Disposables;
using Yona.Core.ViewModels.TrackPanel;
using Yona.Desktop.Extensions;

namespace Yona.Desktop.Views.TrackPanel;

public partial class TrackPanelView : ReactiveUserControl<TrackPanelViewModel>
{
    public TrackPanelView()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            this.ViewModel!.ShowSelectFile.RegisterHandler(this.HandleFileSelect).DisposeWith(disposables);
        });
    }
}