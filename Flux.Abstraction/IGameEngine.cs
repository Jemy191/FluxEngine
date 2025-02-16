using DefaultEcs.System;

namespace Flux.Abstraction;

public interface IGameEngine
{
    IServiceProvider ServiceProvider { get; }
    IGameEngine AddRenderSystem<T>() where T : ISystem<float>;
    IGameEngine AddRenderSystem<T>(T system) where T : ISystem<float>;
    IGameEngine AddRenderSystem<T>(Func<IServiceProvider, T> factory) where T : ISystem<float>;
    IGameEngine AddUpdateSystem<T>() where T : ISystem<float>;
    IGameEngine AddUpdateSystem<T>(T system) where T : ISystem<float>;
    IGameEngine AddUpdateSystem<T>(Func<IServiceProvider, T> factory) where T : ISystem<float>;
    IGameEngine AddResourceManager<T>() where T : IFluxResourceManager;
    IGameEngine Instanciate<T>();
    void Run();
    void RunWith<T>();
}
