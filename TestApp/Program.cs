using System.Text.Json;
using Flux.Asset;
using Flux.Asset.AssetImporters;
using Flux.Asset.Assets;
using Flux.Asset.AssetSources;
using ImageMagick;

await using var catalogueFile = File.OpenRead("AssetsCatalogue.json");
await using var catalogueFileNewer = File.OpenRead("AssetsCatalogueNewer.json");

var availableMetadatas = new Dictionary<string, Type>
{
    { "Path", typeof(string) }
};

List<AssetCatalogue> catalogues = [
    new AssetCatalogue(catalogueFile, availableMetadatas),
    new AssetCatalogue(catalogueFileNewer, availableMetadatas)
];

List<AssetSource> assetSources =
[
    new FileSystemAssetSource(catalogues.MaxBy(c => c.BuildVersion)!, "Assets")
];
var assetServices = new AssetsService(assetSources);
assetServices.RegisterImporter<JsonAsset, JsonImporter>();

var cities = await assetServices.Load<JsonAsset>(new Guid("f053d056-8ec2-42c2-a4ed-350934ad9f2e"));

Console.WriteLine(cities!.Root);

var fileSources = assetServices.AssetSources.OfType<FileSystemAssetSource>();

foreach (var fileSource in fileSources)
{
    Console.WriteLine($"File source:");
    PrintAllFile(fileSource.AssetTree);
}

return;

static void PrintAllFile(AssetTree tree, string indent = "")
{
    Console.WriteLine($"{indent}{tree.Name}");
    indent += '\t';
    foreach (var child in tree.Children.Values)
    {
        PrintAllFile(child, indent);
    }
}