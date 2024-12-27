using Dunet;
using Silk.NET.OpenGL;

namespace Flux.Rendering.GLPrimitives
{
    [Union]
    public partial record VertexAttributePointerType
    {
        partial record Float(VertexAttribPointerType Type);
        partial record Int(VertexAttribIType Type);
    }
}