using ReactiveUI;
using System.Reactive.Disposables;
using Yona.Core.Settings;
using Yona.Core.ViewModels;
using Yona.Desktop.Controls;
using System;
using Yona.Desktop.Views.Updates;

namespace Yona.Desktop.Views;

public partial class MainWindow : ReactiveSukiWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();

        this.WhenActivated((CompositeDisposable disp) =>
        {
            this.ViewModel?.Updates.CheckUpdates();
            this.ViewModel?.Updates.WhenAnyValue(x => x.Update).Subscribe(this.OpenUpdateWindow).DisposeWith(disp);
        });
    }

    private void OpenUpdateWindow(Update? update)
    {
        if (update == null)
        {
            return;
        }

        var updateWin = new UpdateWindow() { DataContext = update };
        updateWin.ShowDialog(this);
    }
}