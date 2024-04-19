using System.Text.Json;
using Flux.Engine.AssetInterfaces;

namespace Flux.Engine.Assets;

public class JsonAsset : IJsonAsset, IDisposable
{
    public JsonElement Root { get; }
    
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