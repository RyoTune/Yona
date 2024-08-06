using Avalonia.Controls;
using System;

namespace Yona.Desktop.Controls;

public partial class Tags : ItemsControl
{
    public Tags()
    {
        InitializeComponent();
    }

    protected override Type StyleKeyOverride => typeof(ItemsControl);
}