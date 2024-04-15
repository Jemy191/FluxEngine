using System.Text.Json;
using JetBrains.Annotations;

namespace Flux.Asset;

public class AssetCatalogue
{
    readonly Dictionary<Guid, AssetCatalogueEntry> assetEntries = [];
    static readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerOptions.Default)
    {
        WriteIndented = true,
    };

    [PublicAPI]
    public AssetCatalogue() { }

    private AssetCatalogue(Dictionary<Guid, AssetCatalogueEntry> assetEntries)
    {
        this.assetEntries = assetEntries;
    }

    public AssetCatalogueEntry Get(Guid guid) => assetEntries[guid];
    public void Add(Guid guid, AssetCatalogueEntry entry) => assetEntries.Add(guid, entry);

    public string Serialize() => JsonSerializer.Serialize(assetEntries, jsonSerializerOptions);

    public static AssetCatalogue? Deserialize(Stream stream)
    {
        var values = JsonSerializer.Deserialize<Dictionary<Guid, AssetCatalogueEntry>>(stream);
        if (values is null)
            return null;
        
        return new AssetCatalogue(values);
    }
}