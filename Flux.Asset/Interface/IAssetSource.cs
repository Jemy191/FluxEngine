using JetBrains.Annotations;

namespace Flux.Asset.Interface;

public interface IAssetSource
{

    [MustDisposeResource]
    Stream Open(AssetCatalogueEntry entry);
}