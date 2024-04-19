using Flux.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Silk.NET.OpenGL.Extensions.ImGui;

namespace Flux.ImGuiFlux
{
    public static class ImGuiExtension
    {
        public static IServiceCollection AddImGui(this IServiceCollection services) =>
            services.AddSingleton<ImGuiController>();
        public static IGameEngine AddImGuiRendering(this IGameEngine engine) =>
            engine.AddRenderSystem<ImGuiRenderSystem>();
    }
}