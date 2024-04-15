using System.Text.Json;
using Flux.Asset;
using Flux.Asset.Interface;

namespace TestApp;

class JsonImporter : IAssetImporter<JsonAsset>
{
    public async Task<JsonAsset?> Import(Stream stream)
    {
        var document = await JsonDocument.ParseAsync(stream);
        return new JsonAsset(document);
    }
}