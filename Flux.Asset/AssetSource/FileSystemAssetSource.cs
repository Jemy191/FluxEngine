using Flux.Asset.Interface;
using JetBrains.Annotations;
using Path = Flux.Core.Path;

namespace Flux.Asset.AssetSource;

public class FileSystemAssetSource : IAssetSource
{
    readonly Path folder;
    public FileSystemAssetSource(Path folder)
    {
        this.folder = folder;
    }

    [MustDisposeResource]
    public Stream Open(AssetCatalogueEntry entry)
    { 
        return File.OpenRead(folder / entry.Path);
    }
}