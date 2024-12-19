using Flux.Abstraction;
using Flux.Rendering.Services;
using Flux.Rendering.Systems;
using Microsoft.Extensions.DependencyInjection;
using Silk.NET.Core.Contexts;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;

namespace Flux.Rendering.Extensions;

public static class RenderingExtension
{
    public static IServiceCollection AddOpenGL<T>(this IServiceCollection services) where T : IGLContextSource => services.AddSingleton(p => p.GetRequiredService<T>().CreateOpenGL());

    public static IServiceCollection AddImGui(this IServiceCollection services) => services.AddSingleton<ImGuiController>();
    public static IServiceCollection AddResourceServices(this IServiceCollection services) => services
        .AddSingleton<ModelLoaderService>()
        .AddSingleton<LoadingService>();
    
    public static IServiceCollection AddModelEntityBuilder(this IServiceCollection services) => services.AddSingleton<ModelEntityBuilderService>();

    public static IGameEngine AddModelRendering(this IGameEngine engine) =>
        engine.AddRenderSystem<ModelRenderSystem>();
    
    /// <summary>
    /// Should be added before other rendering system.
    /// It do GlClear
    /// </summary>
    public static IGameEngine AddOpenGlRendering(this IGameEngine engine) =>
        engine.AddRenderSystem<OpenGlRenderSystem>();
    
    public static IServiceCollection AddOpenGL(this IServiceCollection services) =>
        services.AddSingleton(p => p.GetRequiredService<IWindow>().CreateOpenGL());
}