using Microsoft.Extensions.DependencyInjection;

namespace Flux.Resources;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddResourcesManagement(this IServiceCollection services) => services.AddSingleton<ResourcesRepository>();
}