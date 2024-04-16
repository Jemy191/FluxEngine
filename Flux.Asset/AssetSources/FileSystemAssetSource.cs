using JetBrains.Annotations;
using Path = Flux.Core.Path;

namespace Flux.Asset.AssetSources;

public class FileSystemAssetSource : AssetSource
{
    [PublicAPI]
    public readonly AssetTree assetTree;
    readonly Path rootDirectory;
    
    public FileSystemAssetSource(AssetCatalogue catalogue, Path rootDirectory) : base(catalogue)
    {
        catalogue.HasMetadata<string>("Path");
        this.rootDirectory = rootDirectory;

        assetTree = new AssetTree
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

                var pathParts = path.Split('/');
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
            var currentAssetTree = assetTree;
            foreach (var directory in catalogueAsset.ParentDirectories)
            {
                if (!currentAssetTree.Children.TryGetValue(directory, out var child))
                {
                    child = new AssetTree
                    {
                        Name = directory,
                        CatalogueAsset = null
                    };
                    assetTree.Children.Add(directory, child);
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