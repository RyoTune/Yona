using Avalonia;
using Avalonia.Controls;

namespace Yona.Desktop.Controls;

public partial class Page : UserControl
{
    public static readonly StyledProperty<object?> TitleProperty =
        AvaloniaProperty.Register<Page, object?>(nameof(Title));

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

    public object? Title
    {
        get => this.GetValue(TitleProperty);
        set
        {
            if (value is string text)
            {
                var textBlock = new TextBlock()
                {
                    Text = text,
                    TextTrimming = Avalonia.Media.TextTrimming.CharacterEllipsis,
                    TextWrapping = Avalonia.Media.TextWrapping.NoWrap,
                };

                textBlock.Classes.Add("h2");

                this.SetValue(TitleProperty, textBlock);
            }
            else
            {
                this.SetValue(TitleProperty, value);
            }
        }
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