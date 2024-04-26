using System.Text.Json;
using Flux.Asset.Interface;
using Flux.Engine.Assets;

namespace Flux.Engine.AssetImporters;

public class JsonImporter : IAssetImporter
{
    public IEnumerable<string> SupportedFileFormats => ["json"];
    
    public async Task<Asset.SourceAsset?> Import(Stream stream, Guid guid, string name, string format)
    {
        var document = await JsonDocument.ParseAsync(stream);
        return new JsonAsset(document);
    }
}