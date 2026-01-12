using System.Numerics;
using System.Runtime.InteropServices;
using Flux.Rendering.GLPrimitives;
using JetBrains.Annotations;
using Silk.NET.OpenGL;

namespace Flux.Rendering;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public unsafe readonly struct SimpleVertex : IVertexLayout
{
    static readonly uint vec3Size = (uint)sizeof(Vector3);
    public required Vector3 Position { get; init; }
    public required Vector2 TexCoords { get; init; }

    public static IEnumerable<VertexAttributeData> GetVertexAttributesLayout() =>
    [
        new VertexAttributeData(0, 3, VertexAttribPointerType.Float, vec3Size * 0),
        new VertexAttributeData(1, 2, VertexAttribPointerType.Float, vec3Size * 1)
    ];
}