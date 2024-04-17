using JetBrains.Annotations;

namespace Flux.Asset;

public abstract class AssetSource
{
    public abstract string Name { get; }
    readonly AssetCatalogue catalogue;
    public DateTimeOffset BuildVersion => catalogue.BuildVersion; 
    protected AssetSource(AssetCatalogue catalogue)
    {
        this.catalogue = catalogue;
    }

    public bool ContainAsset(Guid guid) => catalogue.Contain(guid);
    
    [MustDisposeResource]
    public Stream Open(Guid guid) => Open(catalogue.Get(guid));
    [MustDisposeResource]
    protected abstract Stream Open(CatalogueAsset entry);
}