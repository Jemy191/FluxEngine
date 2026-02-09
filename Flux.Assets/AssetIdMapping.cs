using System.Collections.Frozen;
using Flux.Assets.Interfaces;

namespace Flux.Assets;

public class AssetIdMapping
{
    public readonly FrozenDictionary<Guid, AssetInfo> Mapping;
    public AssetIdMapping(Dictionary<Guid, AssetInfo> mapping) => Mapping = mapping.ToFrozenDictionary();

    internal AssetInfo GetAssetInfo(IAssetId id) => Mapping[id.Id];
}