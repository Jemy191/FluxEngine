using Flux.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace Flux.Engine.Services;

public class InjectionService : IInjectionService
{
    readonly IServiceProvider serviceProvider;

    public InjectionService(IServiceProvider serviceProvider) => this.serviceProvider = serviceProvider;

    public T Instantiate<T>() => ActivatorUtilities.CreateInstance<T>(serviceProvider);
}
