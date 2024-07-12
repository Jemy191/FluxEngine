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

    public CatalogueAsset Get(Guid guid) => catalogue.Get(guid);
    
    public bool ContainAsset(Guid guid) => catalogue.Contain(guid);
    
    [MustDisposeResource]
    public Stream Open(Guid guid) => Open(Get(guid));
    [MustDisposeResource]
    protected abstract Stream Open(CatalogueAsset entry);
}