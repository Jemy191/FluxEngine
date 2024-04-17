using System.Numerics;

namespace Flux.MathAddon;

public readonly struct Vertex
{
    public required Vector3 Position {get; init;}
    public required Vector3 Normal {get; init;}
    public required Vector3 Tangent {get; init;}
    public required Vector3 Bitangent {get; init;}
    public required Vector2 TexCoords {get; init;}
    public required Vector3 Colors {get; init;}
}
