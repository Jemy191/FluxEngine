using Flux.Asset;
using Flux.MathAddon;

namespace Flux.Engine.AssetInterfaces;

public interface IMeshAsset : IAsset
{
    IReadOnlyList<uint> Indices { get; }
    IReadOnlyList<Vertex> Vertices { get; }
}