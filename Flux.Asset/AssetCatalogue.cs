using System.Text.Json;
using JetBrains.Annotations;

namespace Flux.Asset;

public class AssetCatalogue
{
    public readonly DateTimeOffset CatalogueBuildVersion;
    readonly Dictionary<Guid, AssetCatalogueEntry> entries;
    readonly Dictionary<string, Type> availableMetadatas;

    static readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerOptions.Default)
    {
        WriteIndented = true
    };

    public AssetCatalogue(Stream stream, Dictionary<string, Type> availableMetadatas)
    {
        this.availableMetadatas = availableMetadatas;
        using var jsonDocument = JsonDocument.Parse(stream);
        CatalogueBuildVersion = jsonDocument.RootElement.GetProperty("BuildVersion").GetDateTimeOffset();
        var values = jsonDocument.RootElement.GetProperty("Entries").Deserialize<Dictionary<Guid, Dictionary<string, JsonElement>>>();
        if (values is null)
        {
            entries = new Dictionary<Guid, AssetCatalogueEntry>();
            return;
        }
        
        entries = values.ToDictionary(v => v.Key, v => ResolveMetadata(v.Value));
    }

    public AssetCatalogueEntry Get(Guid guid) => entries[guid];
    public bool Contain(Guid guid) => entries.ContainsKey(guid);

    public void AddMetadataType<T>(string name) => availableMetadatas.Add(name, typeof(T));
    public bool HasMetadata<T>(string name) => availableMetadatas.TryGetValue(name, out var type) && type == typeof(T);

    public string Serialize() => throw new NotImplementedException();//JsonSerializer.Serialize(assetEntries, jsonSerializerOptions);

    AssetCatalogueEntry ResolveMetadata(Dictionary<string, JsonElement> metadatas)
    {
        var deserializedMetadatas = new Dictionary<string, object>();
        foreach (var (name, jsonValue) in metadatas)
        {
            if(!availableMetadatas.TryGetValue(name, out var metadataType))
                continue;
            
            var value = jsonValue.Deserialize(metadataType);

            if (value is null)
                continue;
            
            deserializedMetadatas.Add(name, value);
        }
        
        return new AssetCatalogueEntry(deserializedMetadatas);
    }
}