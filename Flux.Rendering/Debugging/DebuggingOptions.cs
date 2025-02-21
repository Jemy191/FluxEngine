using Flux.Rendering.GLPrimitives;
using Flux.Resources;

namespace Flux.Rendering.Debugging
{
    public class DebuggingOptions
    {
        public required Resource<Shader> DebugLineShaderId { get; init; }
        public required Resource<Material> DebugLineMaterialId { get; init; }
    }
}
