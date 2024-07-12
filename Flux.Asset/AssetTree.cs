namespace Flux.Asset;

public class AssetTree
{
    public required string Name { get; init; }
    public required CatalogueAsset? CatalogueAsset { get; init; }
    public readonly Dictionary<string, AssetTree> Children = [];
}