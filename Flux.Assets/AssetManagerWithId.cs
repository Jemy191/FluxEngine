using Flux.Assets.Interfaces;
using Microsoft.Extensions.Options;

namespace Flux.Assets;

public class AssetManagerWithId : AssetManager
{
    readonly AssetIdMapping assetIdMapping;
    public const string IdExtension = "guid";

    public AssetManagerWithId(IEnumerable<IAssetLoader> loaders, IOptions<AssetManagerOptions> options, AssetIdMapping assetIdMapping)
        : base(loaders, options) => this.assetIdMapping = assetIdMapping;

    public AssetId<T> GetId<T>(AssetInfo assetInfo)
    {
        _ = GetLoader<T>(assetInfo);

        var fileInfo = ResolvePath(assetInfo);

        var idFilePath = $"{fileInfo.FullName}.{IdExtension}";
        if (!File.Exists(idFilePath))
            throw new FileNotFoundException($"Asset file {fileInfo.FullName} is missing its guid file");

        return new AssetId<T>(Guid.Parse(File.ReadAllText(idFilePath)));
    }

    public AssetHandle<T> Load<T>(AssetId<T> id) => Load<T>(assetIdMapping.GetAssetInfo(id));
}