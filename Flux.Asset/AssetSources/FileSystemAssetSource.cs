using Flux.Asset.Interface;
using JetBrains.Annotations;
using Path = Flux.Core.Path;

namespace Flux.Asset.AssetSources;

public class FileSystemAssetSource : AssetSource
{
    readonly Path folder;
    public FileSystemAssetSource(AssetCatalogue catalogue, Path folder) : base(catalogue)
    {
        catalogue.HasMetadata<string>("Path");
        this.folder = folder;
    }

    [MustDisposeResource]
    protected override Stream Open(AssetCatalogueEntry entry)
    {
        if (!entry.TryGetMetadata<string>("Path", out var path))
            throw new KeyNotFoundException($"Path metadata not found.");
        
        return File.OpenRead(folder / path);
    }
}