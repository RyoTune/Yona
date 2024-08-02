using Avalonia;
using Avalonia.Controls;

namespace Yona.Desktop.Controls;

public partial class Page : UserControl
{
    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<Page, string?>(nameof(Title));

    public static readonly DirectProperty<Page, object?> MenuItemsProperty =
        AvaloniaProperty.RegisterDirect<Page, object?>(nameof(MenuItems), o => o.MenuItems, (o, v) => o.MenuItems = v);

    public static readonly DirectProperty<Page, object?> IconProperty =
        AvaloniaProperty.RegisterDirect<Page, object?>(nameof(Icon), o => o.Icon, (o, v) => o.Icon = v);

    private object? menuItems;
    private object? icon;

    public Page()
    {
        InitializeComponent();
    }

    public string? Title
    {
        get => this.GetValue(TitleProperty);
        set => this.SetValue(TitleProperty, value);
    }

    public object? MenuItems
    {
        get => this.menuItems;
        set => this.SetAndRaise(MenuItemsProperty, ref this.menuItems, value);
    }

    public object? Icon
    {
        get => this.icon;
        set => this.SetAndRaise(IconProperty, ref this.icon, value);
    }
}