using DefaultEcs.System;
using Flux.Abstraction;
using Flux.Ecs;
using Microsoft.Extensions.DependencyInjection;
using Silk.NET.Windowing;

namespace Flux.Engine;

public class GameEngine : IGameEngine
{
    readonly List<ISystem<float>> updater = new();
    readonly List<ISystem<float>> renderer = new();

    readonly IWindow window;
    readonly IInjectionService injectionService;
    SequentialSystem<float> sequentialUpdateSystem;
    SequentialSystem<float> sequentialRenderSystem;

    public GameEngine(IWindow window, IInjectionService injectionService)
    {
        this.window = window;
        this.injectionService = injectionService;

        window.Closing += OnClose;
        window.Render += OnRender;
        window.Update += OnUpdate;

    }

    public T Instantiate<T>()
    {
        return injectionService.Instantiate<T>();
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

    void OnUpdate(double deltaTime) => sequentialUpdateSystem.Update((float)deltaTime);

    void OnRender(double deltaTime) => sequentialRenderSystem.Update((float)deltaTime);

    void OnClose()
    {
        window.Closing -= OnClose;
        window.Render -= OnRender;
        window.Update -= OnUpdate;

        sequentialUpdateSystem.Dispose();
        sequentialRenderSystem.Dispose();
    }

    public async Task RunWith<T>() where T : IGame
    {
        var game = Instantiate<T>();
        await game.Initialize();
        Run();
    }
}