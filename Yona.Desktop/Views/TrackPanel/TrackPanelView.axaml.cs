using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Yona.Core.ViewModels.CreateTrack;
using Yona.Core.ViewModels.TrackPanel;
using Yona.Desktop.Extensions;
using Yona.Desktop.Views.CreateTrack;

namespace Yona.Desktop.Views.TrackPanel;

public partial class TrackPanelView : ReactiveUserControl<TrackPanelViewModel>
{
    public TrackPanelView()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            this.ViewModel?.ShowSelectFile.RegisterHandler(this.HandleFileSelect).DisposeWith(disposables);
            this.ViewModel?.EditTrack.RegisterHandler(this.HandleEditTrack).DisposeWith(disposables);
        });
    }

    private async Task HandleEditTrack(IInteractionContext<CreateTrackViewModel, Unit> context)
    {
        var createTrackWin = new CreateTrackWindow() { DataContext = context.Input };
        await createTrackWin.ShowDialog((Window)TopLevel.GetTopLevel(this)!);
        context.SetOutput(new());
    }
}