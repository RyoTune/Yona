using Avalonia.Controls;
using Avalonia.Interactivity;
using SukiUI.Controls;
using Yona.Library.ViewModels.Dashboard;

namespace Yona.Desktop.Views.Dashboard;

public partial class DashboardView : UserControl
{
    public DashboardView()
    {
        InitializeComponent();
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