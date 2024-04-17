using Flux.Asset.Interface;
using JetBrains.Annotations;

namespace Flux.Asset;

public class AssetsService
{
    readonly List<AssetSource> assetSources;
    public IEnumerable<AssetSource> AssetSources => assetSources;
    readonly Dictionary<string, IAssetImporter> assetImportersByFileFormat = [];
    public AssetsService(List<AssetSource> assetSources)
    {
        this.assetSources = assetSources;
    }
    
    public void RegisterImporter<TAsset, TImporter>() where TAsset : Asset where TImporter : IAssetImporter<TAsset>, new()
    {
        var importer = new TImporter();
        foreach (var format in importer.SupportedFileFormats)
        {
            assetImportersByFileFormat.Add(format, importer);
        }
    }

    public void AddSource(AssetSource source) => assetSources.Add(source);
    
    public async Task<T?> Load<T>(Guid guid) where T : Asset
    {
        var assetSource = assetSources.SingleOrDefault(s => s.ContainAsset(guid));
        
        if (assetSource is null)
            return null;
        
        var catalogueAsset = assetSource.Get(guid);
        
        var importer = assetImportersByFileFormat[catalogueAsset.Format] as IAssetImporter<T>;

        if (importer is null)
            return null;
        
        await using var stream = assetSource.Open(guid);

        return await importer.Import(stream);
    }
}