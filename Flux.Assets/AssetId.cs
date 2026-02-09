using System.Runtime.InteropServices;
using Flux.Assets.Interfaces;

namespace Flux.Assets;

[StructLayout(LayoutKind.Auto)]
public readonly record struct AssetId<T> : IAssetId
{
    public Guid Id { get; }
    public AssetId(Guid id) => Id = id;
}