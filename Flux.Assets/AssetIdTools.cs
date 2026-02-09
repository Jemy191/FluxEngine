namespace Flux.Assets;

public static class AssetIdTools
{
    /// <summary>
    /// This could be called
    /// </summary>
    public static AssetIdMapping GenerateMapping(DirectoryInfo assetsDirectory, string mappingExtension = ".mapping")
    {
        if (!assetsDirectory.Exists)
            throw new DirectoryNotFoundException($"Assets directory {assetsDirectory.FullName} does not exist.");

        var files = assetsDirectory
            .GetFiles("*", SearchOption.AllDirectories)
            .Where(f => f.Extension != ".guid")
            .Where(f => f.Extension != mappingExtension);

        Dictionary<Guid, AssetInfo> mapping = [];

        foreach (var file in files)
        {
            var idFile = new FileInfo($"{file.FullName}.{AssetManagerWithId.IdExtension}");

            if (!idFile.Exists)
                continue;

            var id = Guid.Parse(File.ReadAllText(idFile.FullName));

            // We could use TryAdd() to check collisions, but it's not worth it.
            mapping[id] = Path.GetRelativePath(assetsDirectory.FullName, file.FullName).ToAsset();
        }

        return new AssetIdMapping(mapping);
    }

    public static AssetIdMapping LoadMappingFromFile(FileInfo mappingFile)
    {
        Dictionary<Guid, AssetInfo> mapping = [];

        foreach (var line in File.ReadAllLines(mappingFile.FullName))
        {
            const int guidLength = 36;
            const int assetFileIndex = guidLength + 3;// The 3 is " = "

            var guidText = line[..guidLength];
            var assetPath = line[assetFileIndex..];

            if (!Guid.TryParse(guidText, out var id))
                throw new Exception($"Invalid guid in mapping file: {guidText}");

            mapping[id] = assetPath.ToAsset();
        }

        return new AssetIdMapping(mapping);
    }
}