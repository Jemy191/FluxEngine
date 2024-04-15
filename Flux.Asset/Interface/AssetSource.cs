using JetBrains.Annotations;

namespace Flux.Asset.Interface;

public abstract class AssetSource
{
    readonly AssetCatalogue catalogue;
    public DateTimeOffset CatalogueBuildVersion => catalogue.CatalogueBuildVersion; 
    protected AssetSource(AssetCatalogue catalogue)
    {
        this.catalogue = catalogue;
    }

    public bool ContainAsset(Guid guid) => catalogue.Contain(guid);
    
    [MustDisposeResource]
    public Stream Open(Guid guid) => Open(catalogue.Get(guid));
    [MustDisposeResource]
    protected abstract Stream Open(AssetCatalogueEntry entry);
}