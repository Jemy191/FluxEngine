using Flux.Asset;
using Flux.MathAddon;

namespace Flux.Engine.Assets;

public class MeshAsset : SourceAsset
{
    public required IReadOnlyList<uint> Indices { get; init; }
    public required IReadOnlyList<Vertex> Vertices { get; init; }
}