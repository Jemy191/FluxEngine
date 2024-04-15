using System.Text.Json;
using Flux.Asset;

namespace TestApp;

class JsonAsset : Asset
{
    readonly JsonDocument document;
    public readonly JsonElement Root;

    public JsonAsset(JsonDocument document)
    {
        this.document = document;
        Root = document.RootElement;
    }
    
    public override void Dispose()
    {
        document.Dispose();
    }
}