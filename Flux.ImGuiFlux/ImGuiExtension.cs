using Flux.Abstraction;

namespace Flux.ImGuiFlux;

public static class ImGuiExtension
{
    public static IGameEngine AddImGuiRendering(this IGameEngine engine) =>
        engine.AddRenderSystem<ImGuiRenderSystem>();
}