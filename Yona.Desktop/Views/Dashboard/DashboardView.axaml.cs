using Avalonia.Interactivity;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SukiUI.Controls;
using System;
using System.Reactive.Disposables;
using Yona.Core.Settings.Models;
using Yona.Core.ViewModels.Dashboard;
using Yona.Core.ViewModels.Dashboard.Projects;

namespace Yona.Desktop.Views.Dashboard;

public partial class DashboardView : ReactiveUserControl<DashboardViewModel>
{
    public DashboardView()
    {
        InitializeComponent();

        this.WhenActivated((CompositeDisposable disposables) =>
        {
            this.ViewModel!.Projects.Router.CurrentViewModel
            .Subscribe(x =>
            {
                if (x is ProjectTracksViewModel && this.sukiSideMenu.SelectedItem is not ProjectsViewModel)
                {
                    this.sukiSideMenu.SelectedIndex = (int)Page.Projects;
                }
            })
            .DisposeWith(disposables);
        });
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        if (this.DataContext is DashboardViewModel vm)
        {
            this.FixSideMenu(vm);
            this.SetStartPage(vm);
        }
    }

    /// <summary>
    /// Fixes SideMenuItem icons missing if menu starts closed.
    /// </summary>
    private void FixSideMenu(DashboardViewModel vm)
    {
        foreach (var item in this.sukiSideMenu.Items)
        {
            ((SukiSideMenuItem)item!).IsTopMenuExpanded = vm.IsMenuExpanded;
        }
    }

    /// <summary>
    /// Sets the starting page.
    /// </summary>
    private void SetStartPage(DashboardViewModel vm)
    {
        this.sukiSideMenu.SelectedIndex = (int)vm.StartPage % this.sukiSideMenu.ItemCount;
    }
}