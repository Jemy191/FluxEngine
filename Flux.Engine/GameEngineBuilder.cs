using Flux.Abstraction;
using Flux.Ecs;
using Flux.Engine.Services;
using Microsoft.Extensions.DependencyInjection;
using Silk.NET.Windowing;

namespace Flux.Engine;

public class GameEngineBuilder
{
    public readonly IServiceCollection Services;
    readonly GameEngineService gameEngineService = new GameEngineService();

    public GameEngineBuilder(string name)
    {
        var windowOptions = WindowOptions.Default;
        windowOptions.Title = name;
        windowOptions.Samples = 4;
        windowOptions.WindowState = WindowState.Maximized;
        
#pragma warning disable CA2000
        var window = Window.Create(windowOptions);
#pragma warning restore CA2000
        
        window.Initialize();

        Services = new ServiceCollection()
            .AddSingleton(window)
            .AddSingleton<IView>(window)
            .AddSingleton<IInjectionService, InjectionService>(p => new InjectionService(p))
            .AddSingleton<IEcsWorldService, EcsWorldService>()
            .AddSingleton(gameEngineService);
    }

    public IGameEngine Build()
    {
        var providerOptions = new ServiceProviderOptions
        {
            ValidateScopes = true,
            ValidateOnBuild = true
        };

        var provider = Services.BuildServiceProvider(providerOptions);
        var engine = ActivatorUtilities.CreateInstance<GameEngine>(provider);

        gameEngineService.GameEngine = engine;
        return engine;
    }
}