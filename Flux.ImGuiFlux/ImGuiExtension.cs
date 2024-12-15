using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flux.Abstraction;

namespace Flux.ImGuiFlux;

public static class ImGuiExtension
{
    public static IGameEngine AddImGuiRendering(this IGameEngine engine) =>
        engine.AddRenderSystem<ImGuiRenderSystem>();
}