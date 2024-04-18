using System.Text.Json;
using Flux.Asset.Interface;
using Flux.Engine.Assets;

namespace Flux.Engine.AssetImporters;

public class JsonImporter : IAssetImporter<JsonAsset>
{
    public IEnumerable<string> SupportedFileFormats => ["json"];
    
    public async Task<JsonAsset?> Import(Stream stream, string name, string format)
    {
        var document = await JsonDocument.ParseAsync(stream);
        return new JsonAsset(document);
    }
}