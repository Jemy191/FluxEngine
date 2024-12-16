using DefaultEcs.System;
using Flux.Abstraction;
using Flux.Ecs;
using Silk.NET.Windowing;
using StackExchange.Profiling;

namespace Flux.Engine;

public class GameEngine : IGameEngine
{
    readonly List<ISystem<float>> updater = [];
    readonly List<ISystem<float>> renderer = [];

    readonly IWindow window;
    readonly IInjectionService injectionService;
    readonly MiniProfiler profiler;
    SequentialSystem<float> sequentialUpdateSystem = null!;
    SequentialSystem<float> sequentialRenderSystem = null!;

    public GameEngine(IWindow window, IInjectionService injectionService, MiniProfiler profiler)
    {
        this.window = window;
        this.injectionService = injectionService;
        this.profiler = profiler;

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
        renderer.Add(injectionService.InstanciateSystem<float, T>());
        return this;
    }

    public IGameEngine AddUpdateSystem<T>() where T : ISystem<float>
    {
        updater.Add(injectionService.InstanciateSystem<float, T>());
        return this;
    }

    public void Run()
    {
        sequentialUpdateSystem = new SequentialSystem<float>(updater);
        sequentialRenderSystem = new SequentialSystem<float>(renderer);
        window.Run();
    }

    void OnUpdate(double deltaTime)
    {
        using var _ = profiler.Step("Main update");
        sequentialUpdateSystem.Update((float)deltaTime);
    }

    void OnRender(double deltaTime)
    {
        using var _ = profiler.Step("Main render");
        sequentialRenderSystem.Update((float)deltaTime);
    }

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
        using (profiler.Step("Initialization"))
        {
            Instanciate<T>();
        }
        Run();
    }
}