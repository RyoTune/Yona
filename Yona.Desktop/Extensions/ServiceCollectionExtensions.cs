using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using Yona.Core.App;
using Yona.Core.Projects;
using Yona.Core.Settings;
using Yona.Core.ViewModels;
using Yona.Core.ViewModels.Dashboard;
using Yona.Core.ViewModels.Dashboard.Convert;
using Yona.Core.ViewModels.Dashboard.Home;
using Yona.Core.ViewModels.Dashboard.Projects;
using Yona.Core.ViewModels.Dashboard.Settings;

namespace Yona.Desktop.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddViewModels(this IServiceCollection service)
    {
        service.AddSingleton<MainWindowViewModel>();
        service.AddSingleton<DashboardViewModel>();
        service.AddSingleton<HomeViewModel>();
        service.AddSingleton<ProjectsViewModel>();
        service.AddSingleton<ConvertViewModel>();
        service.AddSingleton<SettingsViewModel>();

        return service;
    }

    public static IServiceCollection AddServices(this IServiceCollection service)
    {
        service.AddSingleton<AppService>();
        service.AddSingleton<SettingsService>();
        service.AddSingleton<TemplatesRepository>();
        service.AddSingleton<ProjectsRepository>();

        return service;
    }

    public static IServiceCollection AddLogger(this IServiceCollection service)
    {
        var appDir = AppDomain.CurrentDomain.BaseDirectory;
        var logsDir = Directory.CreateDirectory(Path.Join(appDir, "logs"));

        var logFiles = logsDir.GetFiles().OrderBy(x => x.LastWriteTime).ToArray();
        if (logFiles.Length > 4)
        {
            File.Delete(logFiles[0].FullName);
        }

        var logFile = Path.Join(logsDir.FullName, $"{DateTime.Now:yyyyMMddTHHmmss}.txt");

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(logFile, outputTemplate: "{Timestamp:HH:mm:ss} [{Level}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        var settingsLog = new SerilogLoggerFactory(Log.Logger).CreateLogger("logger");
        service.AddSingleton(settingsLog);
        Log.Information("Ready.");

        return service;
    }
}
