using Flux.Asset;
using Flux.Asset.AssetSources;
using TestApp;

await using var catalogueFile = File.OpenRead("AssetsCatalogue.json");
await using var catalogueFileNewer = File.OpenRead("AssetsCatalogueNewer.json");

var availableMetadatas = new Dictionary<string, Type>
{
    { "Path", typeof(string) }
};
var catalogue = new AssetCatalogue(catalogueFile, availableMetadatas);
var catalogueNewer = new AssetCatalogue(catalogueFileNewer, availableMetadatas);

List<AssetSource> assetSources =
[
    new FileSystemAssetSource(catalogue, "Assets"),
    new FileSystemAssetSource(catalogueNewer, "Assets")
];
var assetService = new AssetsService(assetSources);
assetService.RegisterImporter<JsonAsset>(new JsonImporter());

var fileSources = assetService.AssetSources.OfType<FileSystemAssetSource>();

foreach (var fileSource in fileSources)
{
    Console.WriteLine($"File source:");
    PrintAllFile(fileSource.assetTree);
}

var cities = await assetService.Load<JsonAsset>(new Guid("f053d056-8ec2-42c2-a4ed-350934ad9f2e"));

//Console.WriteLine(cities!.Root);

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