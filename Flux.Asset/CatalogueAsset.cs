using System.Diagnostics.CodeAnalysis;

namespace Flux.Asset;

public class CatalogueAsset
{
    public readonly string Format;
    public readonly string Name;
    public readonly IReadOnlyDictionary<string, object> Metadatas;
    
    internal CatalogueAsset(string name, string format, IReadOnlyDictionary<string, object> metadatas)
    {
        Name = name;
        Format = format;
        Metadatas = metadatas;
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