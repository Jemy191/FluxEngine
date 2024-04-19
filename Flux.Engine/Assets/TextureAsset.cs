using Flux.Engine.AssetInterfaces;
using Silk.NET.Maths;

namespace Flux.Engine.Assets;

public class TextureAsset : ITextureAsset
{
    public Vector2D<ushort> Size { get; }
    public byte[]? Pixels { get; }

    public TextureAsset(Vector2D<ushort> size, byte[]? pixels)
    {
        this.Size = size;
        this.Pixels = pixels;
    }
}