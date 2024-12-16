using Microsoft.Extensions.DependencyInjection;

namespace Flux.Profiler;

public static class ProfilerExtensions
{
    public static IServiceCollection AddProfiler(this IServiceCollection services)
    {
        return services
            .AddSingleton<ProfilerService>()
            .AddSingleton(sp => sp.GetRequiredService<ProfilerService>().Profiler);
    }
}