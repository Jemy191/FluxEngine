using System.Text.Json;
using Flux.Asset.Assets;
using Flux.Asset.Interface;

namespace Flux.Asset.AssetImporters;

public class JsonImporter : IAssetImporter<JsonAsset>
{
    public async Task<JsonAsset?> Import(Stream stream)
    {
        var document = await JsonDocument.ParseAsync(stream);
        return new JsonAsset(document);
    }
}