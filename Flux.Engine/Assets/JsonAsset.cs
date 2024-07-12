using System.Text.Json;
using Flux.Asset;

namespace Flux.Engine.Assets;

public class JsonAsset : SourceAsset, IDisposable
{
    public readonly JsonElement Root;
    
    readonly JsonDocument document;

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