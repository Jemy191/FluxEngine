using Flux.Rendering.GLPrimitives;

namespace Flux.Rendering;

public interface IVertexLayout
{
    abstract static IEnumerable<VertexAttributeData> GetVertexAttributesLayout();
}