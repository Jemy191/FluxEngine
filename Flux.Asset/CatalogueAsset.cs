using System.Diagnostics.CodeAnalysis;

namespace Flux.Asset;

public class CatalogueAsset
{
    public readonly IReadOnlyDictionary<string, object> Metadatas;
    
    internal CatalogueAsset(IReadOnlyDictionary<string, object> metadatas)
    {
        this.Metadatas = metadatas;
    }

    public bool TryGetMetadata<T>(string name, [NotNullWhen(true)]out T? metadata)
    {
        if(Metadatas.TryGetValue(name, out var value))
        {
            metadata = (T)value;
            return true;
        }
        metadata = default;
        return false;
    }
}