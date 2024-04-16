using System;
using Flux.Abstraction;
using Flux.Ecs;
using Flux.Engine.Services;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Silk.NET.Windowing;

namespace Flux.Engine;

public class GameEngineBuilder
{
    public readonly IServiceCollection Services;

    public GameEngineBuilder(string name)
    {
        var windowOptions = WindowOptions.Default;
        windowOptions.Title = name;
        windowOptions.Samples = 4;
        windowOptions.WindowState = WindowState.Maximized;

        var window = Window.Create(windowOptions);
        window.Initialize();

        Services = new ServiceCollection()
            .AddSingleton(window)
            .AddSingleton<IView>(window)
            .AddSingleton<IInjectionService, InjectionService>(p => new InjectionService(p))
            .AddSingleton<IEcsWorldService, EcsWorldService>();
    }

    public IGameEngine Build()
    {
        var providerOptions = new ServiceProviderOptions
        {
            ValidateScopes = true,
            ValidateOnBuild = true
        };

        var provider = Services.BuildServiceProvider(providerOptions);
        return ActivatorUtilities.CreateInstance<GameEngine>(provider);
    }
}
