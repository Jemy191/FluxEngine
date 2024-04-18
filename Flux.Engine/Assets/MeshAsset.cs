using Flux.MathAddon;

namespace Flux.Engine.Assets;

public class MeshAsset : Asset.Asset
{
    public readonly IReadOnlyList<uint> indices;
    public readonly IReadOnlyList<Vertex> vertices;
    
    public MeshAsset(IReadOnlyList<uint> indices, IReadOnlyList<Vertex> vertices)
    {
        this.indices = indices;
        this.vertices = vertices;
    }
}