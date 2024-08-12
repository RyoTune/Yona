using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Reactive.Disposables;
using Yona.Core.ViewModels.CreateTrack;
using Yona.Desktop.Extensions;

namespace Yona.Desktop.Views.CreateTrack;

public partial class CreateTrackView : ReactiveUserControl<CreateTrackViewModel>
{
    public CreateTrackView()
    {
        InitializeComponent();

        this.WhenActivated((CompositeDisposable disp) =>
        {
            this.ViewModel?.Close.RegisterHandler(this.HandleCloseInteraction).DisposeWith(disp);
        });
    }
}