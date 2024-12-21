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

    public GameEngine(IWindow window, IInjectionService injectionService)
    {
        this.window = window;
        this.injectionService = injectionService;

        window.Closing += OnClose;
        window.Render += OnRender;
        window.Update += OnUpdate;
    }

    public IGameEngine Instanciate<T>()
    {
        injectionService.Instanciate<T>();
        return this;
    }

    public IGameEngine AddRenderSystem<T>() where T : ISystem<float>
    {
        rendererCreators.Add(() => injectionService.InstanciateSystem<float, T>());
        return this;
    }

    public IGameEngine AddUpdateSystem<T>() where T : ISystem<float>
    {
        updaterCreators.Add(() => injectionService.InstanciateSystem<float, T>());
        return this;
    }
    
    public IGameEngine AddResourceManager<T>() where T : IFluxResourceManager
    {
        resourceManagerCreators.Add(() => injectionService.Instanciate<T>());
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
        Instanciate<T>();
        Run();
    }
}