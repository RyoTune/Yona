using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Reactive.Disposables;
using Yona.Core.ViewModels.Dashboard.Convert;
using Yona.Desktop.Extensions;

namespace Yona.Desktop.Views.Dashboard.Convert;

public partial class DashboardConvertView : ReactiveUserControl<DashboardConvertViewModel>
{
    public DashboardConvertView()
    {
        InitializeComponent();

        this.WhenActivated((CompositeDisposable disp) =>
        {
            this.ViewModel?.FileSelect.RegisterHandler(this.HandleFileSelect).DisposeWith(disp);
            this.ViewModel?.FolderSelect.RegisterHandler(this.HandleFolderSelect).DisposeWith(disp);
        });
    }
}