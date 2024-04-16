using Flux.Asset.Interface;

namespace Flux.Asset;

public class AssetsService
{
    readonly List<AssetSource> assetSources;
    public IReadOnlyList<AssetSource> AssetSources => assetSources;
    readonly Dictionary<Type, IAssetImporter> assetImporters = [];
    public AssetsService(List<AssetSource> assetSources)
    {
        this.assetSources = assetSources;
    }
    
    public void RegisterImporter<T>(IAssetImporter importer) where T : Asset
    {
        assetImporters.Add(typeof(T), importer);
    }

    public void AddSource(AssetSource source) => assetSources.Add(source);
    
    public async Task<T?> Load<T>(Guid guid) where T : Asset
    {
        var importer = (IAssetImporter<T>)assetImporters[typeof(T)];

        var assetSource = assetSources
            .Where(s => s.ContainAsset(guid))
            .MaxBy(s => s.BuildVersion);

        if (assetSource is null)
            return null;
        
        await using var stream = assetSource.Open(guid);

        return await importer.Import(stream);
    }
}