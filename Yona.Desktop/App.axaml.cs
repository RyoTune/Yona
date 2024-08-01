using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using System;
using Yona.Desktop.Extensions;
using Yona.Desktop.Views;
using Yona.Library.ViewModels;

namespace Yona.Desktop;

public partial class App : Application
{
    private readonly IServiceProvider serviceProvider;

    public App()
    {
        var services = new ServiceCollection();

        services.AddViewModels();

        this.serviceProvider = services.BuildServiceProvider();
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);

            var mainWindow = this.serviceProvider.GetRequiredService<MainWindowViewModel>();
            desktop.MainWindow = new MainWindow
            {
                DataContext = mainWindow,
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}