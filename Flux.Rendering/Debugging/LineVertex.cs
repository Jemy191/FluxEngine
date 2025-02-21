using System.Numerics;
using System.Runtime.InteropServices;
using Flux.Rendering.GLPrimitives;
using JetBrains.Annotations;
using Silk.NET.OpenGL;

namespace Flux.Rendering.Debugging;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
unsafe readonly struct LineVertex : IVertexLayout
{
    static readonly uint vec3Size = (uint)sizeof(Vector3);
    
    public required Vector3 Position { get; init; }

    public static IEnumerable<VertexAttributeData> GetVertexAttributesLayout() =>
    [
        new VertexAttributeData(0, 3, VertexAttribPointerType.Float, vec3Size * 0),
    ];
}