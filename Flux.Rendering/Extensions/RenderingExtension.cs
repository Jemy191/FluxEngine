using Flux.Abstraction;
using Flux.ImGuiFlux;
using Flux.Rendering.ResourceManagers;
using Flux.Rendering.Services;
using Flux.Rendering.Systems;
using Flux.Slang;
using Microsoft.Extensions.DependencyInjection;
using Silk.NET.Core.Contexts;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;

namespace Flux.Rendering.Extensions;

public static class RenderingExtension
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddImGui() => services.AddSingleton<ImguiService>();
        public IServiceCollection AddModelEntityBuilder() => services.AddSingleton<ModelEntityBuilderService>();
        public IServiceCollection AddOpenGL<T>() where T : IGLContextSource => services.
            AddSingleton(p => p.GetRequiredService<T>().CreateOpenGL())
            .AddSingleton<OpenGLRenderService>();

        public IServiceCollection AddLoaderServices(string assetFolder) => services
            .AddSingleton<ModelLoaderService>()
            .AddSingleton<LoadingService>()
            .AddSingleton(new SlangCompiler([assetFolder]));
    }

    extension(IGameEngine engine)
    {
        public IGameEngine AddModelRendering() => engine.AddRenderSystem<ModelRenderSystem>();
        public IGameEngine AddResourceManagers() =>
            engine.AddResourceManager<TextureResourceManager>()
                .AddResourceManager<ShaderResourceManager>()
                .AddResourceManager<MaterialResourceManager>();
    }

}