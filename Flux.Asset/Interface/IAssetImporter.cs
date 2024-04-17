using Flux.Asset.Assets;

namespace Flux.Asset.Interface;

public interface IAssetImporter<T> : IAssetImporter where T : Asset
{
    Task<T?> Import(Stream stream);
}

public interface IAssetImporter
{
    
}