using Flux.MathAddon;

namespace Flux.Engine.Assets;

public class MeshAsset : Asset.Asset
{
    public readonly IReadOnlyList<uint> Indices;
    public readonly IReadOnlyList<Vertex> Vertices;
    
    public MeshAsset(IReadOnlyList<uint> indices, IReadOnlyList<Vertex> vertices)
    {
        Indices = indices;
        Vertices = vertices;
    }
}