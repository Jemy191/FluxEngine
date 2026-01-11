using Flux.Rendering.GLPrimitives;
using Flux.Resources;

namespace Flux.Rendering.Debugging
{
    public class DebuggingOptions
    {
        public required ResourceId<Shader> DebugLineShaderId { get; init; }
        public required ResourceId<Material> DebugLineMaterialId { get; init; }
    }
}
