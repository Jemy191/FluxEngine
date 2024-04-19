using Flux.Asset;
using Silk.NET.Maths;

namespace Flux.Engine.AssetInterfaces;

public interface ITextureAsset : IAsset
{
    Vector2D<ushort> Size { get; }
    byte[]? Pixels { get; }
}