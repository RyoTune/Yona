using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Serilog;
using SukiUI;
using SukiUI.Models;
using System;
using System.Reactive.Linq;
using Yona.Core.Extensions;
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

        this.settings.WhenAnyValue(x => x.Current.ThemeColor, x => x.Current.CustomPrimaryColor, x => x.Current.CustomAccentColor)
            .Throttle(SavableFileExtensions.SAVE_BUFFER_TIME)
            .Select(x => (Theme: x.Item1, CustomPrimary: x.Item2, CustomAccent: x.Item3))
            .Subscribe(settings =>
            {
                var sukiTheme = SukiTheme.GetInstance();
                if (settings.Theme.Name != ColorTheme.Custom.Name)
                {
                    var sukiColor = new SukiColorTheme(settings.Theme.Name, Color.Parse(settings.Theme.PrimaryColor), Color.Parse(settings.Theme.AccentColor));
                    sukiTheme?.ChangeColorTheme(sukiColor);
                }
                else
                {
                    try
                    {
                        var sukiColor = new SukiColorTheme(settings.Theme.Name, Color.Parse(settings.CustomPrimary), Color.Parse(settings.CustomAccent));
                        sukiTheme?.ChangeColorTheme(sukiColor);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Failed to set custom theme colors.");
                    }
                }
            });

        this.settings.WhenAnyValue(x => x.Current.ThemeMode)
            .Subscribe(themeMode =>
            {
                var sukiTheme = SukiTheme.GetInstance();
                if (themeMode == ThemeMode.Dark)
                {
                    sukiTheme.ChangeBaseTheme(ThemeVariant.Dark);
                }
                else if (themeMode == ThemeMode.Light)
                {
                    sukiTheme.ChangeBaseTheme(ThemeVariant.Light);
                }
                else
                {
                    sukiTheme.ChangeBaseTheme(ThemeVariant.Default);
                }
            });
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
}