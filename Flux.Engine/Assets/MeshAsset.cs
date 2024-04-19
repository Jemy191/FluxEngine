using Flux.Engine.AssetInterfaces;
using Flux.MathAddon;

namespace Flux.Engine.Assets;

public class MeshAsset : IMeshAsset
{
    public IReadOnlyList<uint> Indices { get; }
    public IReadOnlyList<Vertex> Vertices { get; }

    public MeshAsset(IReadOnlyList<uint> indices, IReadOnlyList<Vertex> vertices)
    {
        Indices = indices;
        Vertices = vertices;
    }
}