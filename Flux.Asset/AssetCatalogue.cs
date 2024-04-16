using System.Text.Json;

namespace Flux.Asset;

public class AssetCatalogue
{
    public readonly DateTimeOffset BuildVersion;
    readonly Dictionary<Guid, AssetCatalogueEntry> entries;
    readonly Dictionary<string, Type> availableMetadatas;

    static readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerOptions.Default)
    {
        WriteIndented = true
    };

    public AssetCatalogue(Dictionary<Guid, AssetCatalogueEntry> entries, Dictionary<string, Type> availableMetadatas, DateTimeOffset buildVersion)
    {
        this.entries = entries;
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
            entries = new Dictionary<Guid, AssetCatalogueEntry>();
            return;
        }

        entries = values.ToDictionary(v => v.Key, v => ResolveMetadata(v.Value));
    }

    public AssetCatalogueEntry Get(Guid guid) => entries[guid];
    public bool TryAdd(Guid guid, AssetCatalogueEntry entry) => entries.TryAdd(guid, entry);
    public bool Contain(Guid guid) => entries.ContainsKey(guid);

    public bool TryAddMetadataType<T>(string name) => availableMetadatas.TryAdd(name, typeof(T));
    public bool HasMetadata<T>(string name) => availableMetadatas.TryGetValue(name, out var type) && type == typeof(T);

    AssetCatalogueEntry ResolveMetadata(Dictionary<string, JsonElement> metadatas)
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

        return new AssetCatalogueEntry(deserializedMetadatas);
    }

    public string Serialize()
    {
        var catalogue = new {
            BuildVersion = BuildVersion,
            Entries = entries.ToDictionary(e => e.Key, e => e.Value.Metadatas)
        };
        return JsonSerializer.Serialize(catalogue, jsonSerializerOptions);
    }
}