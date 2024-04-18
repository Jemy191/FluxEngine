using Silk.NET.Maths;

namespace Flux.Asset.Assets;

public class TextureAsset : Asset
{
    public readonly Vector2D<ushort> size;
    public readonly byte[]? pixels;
    public TextureAsset(Vector2D<ushort> size, byte[]? pixels)
    {
        this.size = size;
        this.pixels = pixels;
    }
}