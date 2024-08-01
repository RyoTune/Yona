using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System;
using Yona.Library.ViewModels;

namespace Yona.Desktop;

public class ViewLocator : IDataTemplate
{

    public Control? Build(object? data)
    {
        if (data is null)
            return null;

        var name = data.GetType().FullName!.Replace("Yona.Library.ViewModel", "Yona.Desktop.View", StringComparison.Ordinal);
        var type = Type.GetType(name);

        if (type != null)
        {
            var control = (Control)Activator.CreateInstance(type)!;
            control.DataContext = data;
            return control;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}
