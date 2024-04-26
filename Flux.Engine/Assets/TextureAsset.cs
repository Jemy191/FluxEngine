using Flux.Asset;
using Silk.NET.Maths;

namespace Flux.Engine.Assets;

public class TextureAsset : SourceAsset
{
    public required Vector2D<ushort> Size { get; init; }
    public required byte[]? Pixels { get; init; }
}