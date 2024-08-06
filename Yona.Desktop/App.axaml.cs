using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Microsoft.Extensions.DependencyInjection;
using SukiUI;
using SukiUI.Enums;
using System;
using System.ComponentModel;
using Yona.Core.Settings;
using Yona.Core.Settings.Models;
using Yona.Core.ViewModels;
using Yona.Core.ViewModels.Dashboard;
using Yona.Desktop.Extensions;
using Yona.Desktop.Views;

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
        services.AddLogger();

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

        this.SetThemeMode(sukiTheme);
        this.SetThemeColor(sukiTheme);

        settings.Current.PropertyChanged += Settings_PropertyChanged;
    }

    private void Settings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        var sukiTheme = SukiTheme.GetInstance();

        if (e.PropertyName == nameof(this.settings.Current.ThemeMode))
        {
            this.SetThemeMode(sukiTheme);
        }

        if (e.PropertyName == nameof(this.settings.Current.ThemeColor))
        {
            this.SetThemeColor(sukiTheme);
        }
    }

    private void SetThemeMode(SukiTheme sukiTheme)
    {
        if (this.settings.Current.ThemeMode == ThemeMode.Dark)
        {
            sukiTheme.ChangeBaseTheme(ThemeVariant.Dark);
        }
        else if (this.settings.Current.ThemeMode == ThemeMode.Light)
        {
            sukiTheme.ChangeBaseTheme(ThemeVariant.Light);
        }
        else
        {
            sukiTheme.ChangeBaseTheme(ThemeVariant.Default);
        }
    }

    private void SetThemeColor(SukiTheme sukiTheme)
    {
        // Set color theme.
        sukiTheme.ChangeColorTheme(GetSukiColor(this.settings.Current.ThemeColor));
    }

    private static SukiColor GetSukiColor(ThemeColor color) => color switch
    {
        ThemeColor.Blue => SukiColor.Blue,
        ThemeColor.Orange => SukiColor.Orange,
        ThemeColor.Green => SukiColor.Green,
        ThemeColor.Red => SukiColor.Red,
            _ => SukiColor.Blue,
    };
}