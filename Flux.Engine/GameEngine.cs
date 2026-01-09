using DefaultEcs.System;
using Flux.Abstraction;
using Flux.Ecs;
using Silk.NET.Windowing;

namespace Flux.Engine;

public class GameEngine : IGameEngine
{
    readonly List<Func<ISystem<float>>> updaterCreators = [];
    readonly List<Func<ISystem<float>>> rendererCreators = [];
    readonly List<Func<IFluxResourceManager>> resourceManagerCreators = [];
    readonly List<IFluxResourceManager> resourceManagers = [];

    readonly IWindow window;
    readonly IInjectionService injectionService;
    SequentialSystem<float> sequentialUpdateSystem = null!;
    SequentialSystem<float> sequentialRenderSystem = null!;

    public IServiceProvider ServiceProvider { get; }

    public GameEngine(IWindow window, IInjectionService injectionService, IServiceProvider serviceProvider)
    {
        this.window = window;
        this.injectionService = injectionService;
        ServiceProvider = serviceProvider;

        window.Closing += OnClose;
        window.Render += OnRender;
        window.Update += OnUpdate;
    }

    public IGameEngine Instantiate<T>()
    {
        injectionService.Instantiate<T>();
        return this;
    }

    public IGameEngine AddRenderSystem<T>() where T : ISystem<float>
    {
        rendererCreators.Add(() => injectionService.InstantiateSystem<float, T>());
        return this;
    }
    public IGameEngine AddRenderSystem<T>(T system) where T : ISystem<float>
    {
        rendererCreators.Add(() => system);
        return this;
    }
    public IGameEngine AddRenderSystem(Action<float> action)
    {
        rendererCreators.Add(() => new ActionSystem<float>(action));
        return this;
    }

    public IGameEngine AddRenderSystem<T>(Func<IServiceProvider, T> factory) where T : ISystem<float>
    {
        rendererCreators.Add(() => factory.Invoke(ServiceProvider));
        return this;
    }

    public IGameEngine AddUpdateSystem<T>() where T : ISystem<float>
    {
        updaterCreators.Add(() => injectionService.InstantiateSystem<float, T>());
        return this;
    }
    public IGameEngine AddUpdateSystem<T>(T system) where T : ISystem<float>
    {
        updaterCreators.Add(() => system);
        return this;
    }
    public IGameEngine AddUpdateSystem(Action<float> action)
    {
        updaterCreators.Add(() => new ActionSystem<float>(action));
        return this;
    }

    public IGameEngine AddUpdateSystem<T>(Func<IServiceProvider, T> factory) where T : ISystem<float>
    {
        updaterCreators.Add(() => factory.Invoke(ServiceProvider));
        return this;
    }
    
    public IGameEngine AddResourceManager<T>() where T : IFluxResourceManager
    {
        resourceManagerCreators.Add(() => injectionService.Instantiate<T>());
        return this;
    }

    public void Run()
    {
        resourceManagers.AddRange(resourceManagerCreators.Select(rm => rm.Invoke()));
        sequentialUpdateSystem = new SequentialSystem<float>(updaterCreators.Select(u => u.Invoke()));
        sequentialRenderSystem = new SequentialSystem<float>(rendererCreators.Select(r => r.Invoke()));
        window.Run();
    }

    void OnUpdate(double deltaTime) => sequentialUpdateSystem.Update((float)deltaTime);

    void OnRender(double deltaTime) => sequentialRenderSystem.Update((float)deltaTime);

    void OnClose()
    {
        window.Closing -= OnClose;
        window.Render -= OnRender;
        window.Update -= OnUpdate;

        sequentialUpdateSystem.Dispose();
        sequentialRenderSystem.Dispose();

        injectionService.DisposeAsync().AsTask().Wait();
    }

    public void RunWith<T>()
    {
        Instantiate<T>();
        Run();
    }
}