using Silk.NET.Maths;

namespace Flux.Engine.Assets;

public class TextureAsset : Asset.Asset
{
    public readonly Vector2D<ushort> size;
    public readonly byte[]? pixels;
    public TextureAsset(Vector2D<ushort> size, byte[]? pixels)
    {
        this.size = size;
        this.pixels = pixels;
    }
}