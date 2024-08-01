using Microsoft.Extensions.DependencyInjection;
using Yona.Library.ViewModels;

namespace Yona.Desktop.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddViewModels(this IServiceCollection service)
    {
        service.AddSingleton<MainWindowViewModel>();

        return service;
    }
}
