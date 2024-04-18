using System.Text.Json;

namespace Flux.Engine.Assets;

public class JsonAsset : Asset.Asset, IDisposable
{
    readonly JsonDocument document;
    public readonly JsonElement Root;

    public JsonAsset(JsonDocument document)
    {
        this.document = document;
        Root = document.RootElement;
    }
    
    public void Dispose()
    {
        document.Dispose();
    }
}