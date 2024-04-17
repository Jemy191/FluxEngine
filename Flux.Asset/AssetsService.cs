using Flux.Asset.Interface;

namespace Flux.Asset;

public class AssetsService
{
    readonly List<AssetSource> assetSources;
    public IEnumerable<AssetSource> AssetSources => assetSources;
    readonly Dictionary<Type, IAssetImporter> assetImporters = [];
    public AssetsService(List<AssetSource> assetSources)
    {
        this.assetSources = assetSources;
    }
    
    public void RegisterImporter<TAsset, TImporter>() where TAsset : Asset where TImporter : IAssetImporter<TAsset>, new()
    {
        assetImporters.Add(typeof(TAsset), new TImporter());
    }

    public void AddSource(AssetSource source) => assetSources.Add(source);
    
    public async Task<T?> Load<T>(Guid guid) where T : Asset
    {
        var importer = (IAssetImporter<T>)assetImporters[typeof(T)];

        var assetSource = assetSources.SingleOrDefault(s => s.ContainAsset(guid));

        if (assetSource is null)
            return null;
        
        await using var stream = assetSource.Open(guid);

        return await importer.Import(stream);
    }
}