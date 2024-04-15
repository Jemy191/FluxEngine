using Path = Flux.Core.Path;

namespace Flux.Asset.Utils;

public static class AssetCatalogueBuilder
{
    public static AssetCatalogue BuildFromDirectory(Path directory)
    {
        if (!Directory.Exists(directory))
            throw new DirectoryNotFoundException($"Directory do not exist: {directory}");

        var entries = ScanDirectory(directory)
            .ToDictionary(_ => Guid.NewGuid(), e => e);

        return new AssetCatalogue(entries, new Dictionary<string, Type>
        {
            { "Path", typeof(string) }
        }, DateTimeOffset.UtcNow);
    }

    static IEnumerable<AssetCatalogueEntry> ScanDirectory(Path rootDirectory)
    {
        var entries = ScanFiles(Directory.EnumerateFiles(rootDirectory)).ToList();
        
        foreach (var directory in Directory.EnumerateDirectories(rootDirectory))
        {
            entries.AddRange(ScanDirectory(directory));
        }
        return entries;
    }

    static IEnumerable<AssetCatalogueEntry> ScanFiles(IEnumerable<string> files) => files
        .Select(f => new AssetCatalogueEntry(new Dictionary<string, object>
        {
            { "Path", f }
        }));


}