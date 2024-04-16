using System.Text.Json;

namespace Flux.Asset;

public class AssetCatalogue
{
    public readonly DateTimeOffset BuildVersion;
    readonly Dictionary<Guid, CatalogueAsset> catalogueAssets;
    public IReadOnlyDictionary<Guid, CatalogueAsset> CatalogueAssets => catalogueAssets;
    readonly Dictionary<string, Type> availableMetadatas;

    static readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerOptions.Default)
    {
        WriteIndented = true
    };

    public AssetCatalogue(Dictionary<Guid, CatalogueAsset> catalogueAssets, Dictionary<string, Type> availableMetadatas, DateTimeOffset buildVersion)
    {
        this.catalogueAssets = catalogueAssets;
        this.availableMetadatas = availableMetadatas;
        BuildVersion = buildVersion;
    }

    public AssetCatalogue(Stream stream, Dictionary<string, Type> availableMetadatas)
    {
        this.availableMetadatas = availableMetadatas;
        using var jsonDocument = JsonDocument.Parse(stream);
        BuildVersion = jsonDocument.RootElement.GetProperty("BuildVersion").GetDateTimeOffset();
        var values = jsonDocument.RootElement.GetProperty("Entries").Deserialize<Dictionary<Guid, Dictionary<string, JsonElement>>>();
        if (values is null)
        {
            catalogueAssets = new Dictionary<Guid, CatalogueAsset>();
            return;
        }

        catalogueAssets = values.ToDictionary(v => v.Key, v => ResolveMetadata(v.Value));
    }

    public CatalogueAsset Get(Guid guid) => catalogueAssets[guid];
    public bool TryAdd(Guid guid, CatalogueAsset entry) => catalogueAssets.TryAdd(guid, entry);
    public bool Contain(Guid guid) => catalogueAssets.ContainsKey(guid);

    public bool TryAddMetadataType<T>(string name) => availableMetadatas.TryAdd(name, typeof(T));
    public bool HasMetadata<T>(string name) => availableMetadatas.TryGetValue(name, out var type) && type == typeof(T);

    CatalogueAsset ResolveMetadata(Dictionary<string, JsonElement> metadatas)
    {
        var deserializedMetadatas = new Dictionary<string, object>();
        foreach (var (name, jsonValue) in metadatas)
        {
            if (!availableMetadatas.TryGetValue(name, out var metadataType))
                continue;

            var value = jsonValue.Deserialize(metadataType);

            if (value is null)
                continue;

            deserializedMetadatas.Add(name, value);
        }

        return new CatalogueAsset(deserializedMetadatas);
    }

    public string Serialize()
    {
        var catalogue = new {
            BuildVersion = BuildVersion,
            Entries = catalogueAssets.ToDictionary(e => e.Key, e => e.Value.Metadatas)
        };
        return JsonSerializer.Serialize(catalogue, jsonSerializerOptions);
    }
}