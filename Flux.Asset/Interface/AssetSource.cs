using JetBrains.Annotations;

namespace Flux.Asset.Interface;

public abstract class AssetSource
{
    protected readonly AssetCatalogue catalogue;
    protected AssetSource(AssetCatalogue catalogue)
    {
        this.catalogue = catalogue;
    }

    [MustDisposeResource]
    public Stream Open(Guid guid) => Open(catalogue.Get(guid));
    [MustDisposeResource]
    protected abstract Stream Open(AssetCatalogueEntry entry);
}