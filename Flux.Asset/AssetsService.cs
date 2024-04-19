using Flux.Asset.Interface;

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

    /// <summary>
    /// Importer with support for the same file format will override any previous one  
    /// </summary>
    public void RegisterImporter<TAsset, TImporter>(IReadOnlyCollection<string>? extraSupportedFileFormat = null)
        where TAsset : IAsset where TImporter : IAssetImporter, new()
    {
        extraSupportedFileFormat ??= [];
        
        var importer = new TImporter();
        foreach (var format in importer.SupportedFileFormats.Union(extraSupportedFileFormat))
        {
            assetImportersByFileFormat[format] = importer;
        }
    }

    public void AddSource(AssetSource source) => assetSources.Add(source);

    public async Task<T?> Load<T>(Guid guid) where T : IAsset
    {
        var assetSource = assetSources.SingleOrDefault(s => s.ContainAsset(guid));

        if (assetSource is null)
            return default;

        var catalogueAsset = assetSource.Get(guid);

        if (!assetImportersByFileFormat.TryGetValue(catalogueAsset.Format, out var importer))
            return default;

        await using var stream = assetSource.Open(guid);
        
        if (await importer.Import(stream, catalogueAsset.Name, catalogueAsset.Format) is not T asset)
            return default;
        
        return asset;
    }
}