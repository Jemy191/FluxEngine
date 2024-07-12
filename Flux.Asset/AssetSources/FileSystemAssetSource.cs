using JetBrains.Annotations;
using Path = Flux.Core.Path;

namespace Flux.Asset.AssetSources;

public class FileSystemAssetSource : AssetSource
{
    public override string Name => rootDirectory;
    [PublicAPI]
    public readonly AssetTree AssetTree;
    readonly Path rootDirectory;
    
    public FileSystemAssetSource(AssetCatalogue catalogue, Path rootDirectory) : base(catalogue)
    {
        catalogue.HasMetadata<string>("Path");
        this.rootDirectory = rootDirectory;

        AssetTree = new AssetTree
        {
            Name = rootDirectory,
            CatalogueAsset = null,
        };

        FillAssetTree(catalogue);
    }

    [MustDisposeResource]
    protected override Stream Open(CatalogueAsset entry)
    {
        if (!entry.TryGetMetadata<string>("Path", out var path))
            throw new KeyNotFoundException($"Path metadata not found.");

        return File.OpenRead(rootDirectory / path);
    }

    void FillAssetTree(AssetCatalogue catalogue)
    {

        var sortedCatalogueAssets = catalogue.CatalogueAssets
            .Select(ca =>
            {
                if (!ca.Value.TryGetMetadata<string>("Path", out var path))
                    throw new KeyNotFoundException($"Unable to find Path metadata for asset: {ca.Key}");

                var pathParts = path.Split('\\');
                return new
                {
                    Guid = ca.Key,
                    ParentDirectories = pathParts[..^1],
                    Name = pathParts[^1],
                    CatalogueAsset = ca.Value
                };
            })
            .OrderBy(ca => ca.ParentDirectories.Length);

        foreach (var catalogueAsset in sortedCatalogueAssets)
        {
            var currentAssetTree = AssetTree;
            foreach (var directory in catalogueAsset.ParentDirectories)
            {
                if (!currentAssetTree.Children.TryGetValue(directory, out var child))
                {
                    child = new AssetTree
                    {
                        Name = directory,
                        CatalogueAsset = null
                    };
                    AssetTree.Children.Add(directory, child);
                }
                currentAssetTree = child;
            }

            currentAssetTree.Children.Add(catalogueAsset.Name, new AssetTree
            {
                Name = catalogueAsset.Name,
                CatalogueAsset = catalogueAsset.CatalogueAsset
            });
        }
    }
}