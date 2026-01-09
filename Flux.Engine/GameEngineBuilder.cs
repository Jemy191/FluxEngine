using Flux.Abstraction;
using Flux.Ecs;
using Flux.Engine.Services;
using Microsoft.Extensions.DependencyInjection;
using Silk.NET.Windowing;

namespace Flux.Engine;

public class GameEngineBuilder
{
    public readonly IServiceCollection Services;

    /// <param name="windowSize">As a 0-1 percentage.</param>
    public GameEngineBuilder(string name, float windowSize = .5f)
    {
        // The main monitor should always be available.
        // If not, this code should be changed so we can run headless.
        var screenResolution = Window.GetWindowPlatform(false)!.GetMainMonitor().VideoMode.Resolution!.Value;

        var windowOptions = WindowOptions.Default;
        windowOptions.Title = name;
        windowOptions.Samples = 4;
        windowOptions.WindowState = WindowState.Normal;
        windowOptions.Size = (screenResolution.As<float>() * windowSize).As<int>();

        // Make the window floating on Hyprland.
        // HYPRLAND_INSTANCE_SIGNATURE should not exist on non-Hyprland instances.
        if (Environment.GetEnvironmentVariable("HYPRLAND_INSTANCE_SIGNATURE") is not null)
            windowOptions.WindowBorder = WindowBorder.Fixed;

#pragma warning disable CA2000
        var window = Window.Create(windowOptions);
#pragma warning restore CA2000
        
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