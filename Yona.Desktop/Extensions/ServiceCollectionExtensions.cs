using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Yona.Core.App;
using Yona.Core.Audio;
using Yona.Core.Projects;
using Yona.Core.Projects.Builders;
using Yona.Core.Settings;
using Yona.Core.ViewModels;
using Yona.Core.ViewModels.Convert;
using Yona.Core.ViewModels.Dashboard;
using Yona.Core.ViewModels.Dashboard.Convert;
using Yona.Core.ViewModels.Dashboard.Home;
using Yona.Core.ViewModels.Dashboard.Projects;
using Yona.Core.ViewModels.Dashboard.Settings;
using Yona.Core.ViewModels.TrackPanel;
using Yona.Desktop.Common;

namespace Yona.Desktop.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddViewModels(this IServiceCollection service)
    {
        service.AddSingleton<MainWindowViewModel>();
        service.AddSingleton<DashboardViewModel>();
        service.AddSingleton<HomeViewModel>();
        service.AddSingleton<ProjectsViewModel>();
        service.AddSingleton<SettingsViewModel>();
        service.AddSingleton<DashboardConvertViewModel>();

        return service;
    }

    public static IServiceCollection AddServices(this IServiceCollection service)
    {
        service.AddSingleton<AppService>();
        service.AddSingleton<SettingsService>();
        service.AddSingleton<TemplateRepository>();
        service.AddSingleton<ProjectRepository>();
        service.AddSingleton<LoopService>();
        service.AddSingleton<TrackPanelFactory>();
        service.AddSingleton<EncoderRepository>();
        service.AddSingleton<ProjectBuilder>();
        service.AddSingleton<StandardProjectBuilder>();
        service.AddSingleton<FastProjectBuilder>();
        service.AddSingleton<ProjectsRouterFactory>();
        service.AddSingleton<ProjectTracksFactory>();
        service.AddSingleton<ProjectServices>();
        service.AddSingleton<ConvertProjectBuilder>();
        service.AddSingleton<ConvertFactory>();
        service.AddSingleton<UpdateService>(s =>
        {
            var assembly = Assembly.GetExecutingAssembly();
#if DEBUG
            var fileVersionInfo = "1.0.0";
#else
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;
#endif
            return new(s.GetRequiredService<AppService>(), new(fileVersionInfo ?? "0.0.1"));
        });

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
            .WriteTo.Sink(new ToastSink())
            .WriteTo.File(logFile, outputTemplate: "{Timestamp:HH:mm:ss} [{Level}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        var generalLog = new SerilogLoggerFactory(Log.Logger).CreateLogger("General");
        var loggerFactory = LoggerFactory.Create(logging => logging.AddSerilog());

        service.AddSingleton(generalLog);
        service.AddSingleton(s => loggerFactory.CreateLogger<FastProjectBuilder>());
        service.AddSingleton(s => loggerFactory.CreateLogger<StandardProjectBuilder>());
        service.AddSingleton(s => loggerFactory.CreateLogger<ProjectRepository>());
        service.AddSingleton(s => loggerFactory.CreateLogger<TemplateRepository>());
        service.AddSingleton(s => loggerFactory.CreateLogger<EncoderRepository>());
        service.AddSingleton(s => loggerFactory.CreateLogger<HomeViewModel>());
        service.AddSingleton(s => loggerFactory.CreateLogger<TrackPanelViewModel>());
        service.AddSingleton(s => loggerFactory.CreateLogger<ConvertProjectBuilder>());
        service.AddSingleton(s => loggerFactory.CreateLogger<ConvertViewModel>());
        service.AddSingleton(s => loggerFactory.CreateLogger<ProjectTracksViewModel>());

        Log.Information("Ready.");
        return service;
    }
}
