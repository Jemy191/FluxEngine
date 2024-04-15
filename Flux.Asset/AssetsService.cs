using Flux.Asset.Interface;
using Path = Flux.Core.Path;

namespace Flux.Asset;

public class AssetsService
{
    readonly AssetCatalogue catalogue;
    readonly IAssetSource assetSource;
    readonly Dictionary<Type, IAssetImporter> assetImporters = [];
    public AssetsService(AssetCatalogue catalogue, IAssetSource assetSource)
    {
        this.catalogue = catalogue;
        this.assetSource = assetSource;
    }
    
    public void RegisterImporter<T>(IAssetImporter importer) where T : Asset
    {
        assetImporters.Add(typeof(T), importer);
    }
    
    public async Task<T?> Load<T>(Guid guid) where T : Asset
    {
        var importer = (IAssetImporter<T>)assetImporters[typeof(T)];
        
        var entry = catalogue.Get(guid);
        await using var stream = assetSource.Open(entry);

        return await importer.Import(stream);
    }
}