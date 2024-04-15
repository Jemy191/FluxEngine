using System.Diagnostics.CodeAnalysis;

namespace Flux.Asset;

public class AssetCatalogueEntry
{
    readonly IReadOnlyDictionary<string, object> metadatas;
    
    internal AssetCatalogueEntry(IReadOnlyDictionary<string, object> metadatas)
    {
        this.metadatas = metadatas;
    }

    public bool TryGetMetadata<T>(string name, [NotNullWhen(true)]out T? metadata)
    {
        if(metadatas.TryGetValue(name, out var value))
        {
            metadata = (T)value;
            return true;
        }
        metadata = default;
        return false;
    }
}