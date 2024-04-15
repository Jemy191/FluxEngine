using Flux.Asset.Interface;
using Path = Flux.Core.Path;

namespace Flux.Asset;

public class AssetsService
{
    readonly Interface.AssetSource assetSource;
    readonly Dictionary<Type, IAssetImporter> assetImporters = [];
    public AssetsService(Interface.AssetSource assetSource)
    {
        this.assetSource = assetSource;
    }
    
    public void RegisterImporter<T>(IAssetImporter importer) where T : Asset
    {
        assetImporters.Add(typeof(T), importer);
    }
    
    public async Task<T?> Load<T>(Guid guid) where T : Asset
    {
        var importer = (IAssetImporter<T>)assetImporters[typeof(T)];
        
        await using var stream = assetSource.Open(guid);

        return await importer.Import(stream);
    }
}