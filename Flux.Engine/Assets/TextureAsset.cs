using Silk.NET.Maths;

namespace Flux.Engine.Assets;

public class TextureAsset : Asset.Asset
{
    public readonly Vector2D<ushort> Size;
    public readonly byte[]? Pixels;
    public TextureAsset(Vector2D<ushort> size, byte[]? pixels)
    {
        this.Size = size;
        this.Pixels = pixels;
    }
}