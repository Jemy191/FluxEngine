using Flux.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace Flux.Engine.Services;

public class InjectionService : IInjectionService
{
    readonly IServiceProvider serviceProvider;

    bool disposed;

    public InjectionService(IServiceProvider serviceProvider) => this.serviceProvider = serviceProvider;

    public T Instantiate<T>() => ActivatorUtilities.CreateInstance<T>(serviceProvider);

    public async ValueTask DisposeAsync()
    {
        if (disposed)
            return;

        disposed = true;

        if (serviceProvider is IAsyncDisposable disposable)
            await disposable.DisposeAsync();
    }
}