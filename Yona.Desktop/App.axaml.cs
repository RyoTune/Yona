using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using SukiUI;
using SukiUI.Enums;
using System;
using System.ComponentModel;
using Yona.Desktop.Extensions;
using Yona.Desktop.Views;
using Yona.Library.Settings;
using Yona.Library.Settings.Models;
using Yona.Library.ViewModels;
using Yona.Library.ViewModels.Dashboard;

namespace Yona.Desktop;

public partial class App : Application
{
    private readonly IServiceProvider serviceProvider;
    private readonly SettingsService settings;

    public App()
    {
        var services = new ServiceCollection();

        services.AddViewModels();
        services.AddServices();
        services.AddLoggers();

        this.serviceProvider = services.BuildServiceProvider();
        this.settings = this.serviceProvider.GetRequiredService<SettingsService>();
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        this.SetupTheming();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);

            var mainWindow = this.serviceProvider.GetRequiredService<MainWindowViewModel>();
            var dashboard = this.serviceProvider.GetRequiredService<DashboardViewModel>();
            mainWindow.RootViewModel = dashboard;

            desktop.MainWindow = new MainWindow
            {
                DataContext = mainWindow,
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void SetupTheming()
    {
        var sukiTheme = SukiTheme.GetInstance();
        sukiTheme.ChangeColorTheme(this.GetSukiColor(settings.Current.ThemeColor));

        settings.Current.PropertyChanged += Settings_PropertyChanged;
    }

    private void Settings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        var sukiTheme = SukiTheme.GetInstance();
        sukiTheme.ChangeColorTheme(this.GetSukiColor(settings.Current.ThemeColor));
    }

    private SukiColor GetSukiColor(ThemeColor color) => color switch
    {
        ThemeColor.Blue => SukiColor.Blue,
        ThemeColor.Orange => SukiColor.Orange,
        ThemeColor.Green => SukiColor.Green,
        ThemeColor.Red => SukiColor.Red,
            _ => SukiColor.Blue,
    };
}