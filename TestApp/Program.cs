using Flux.Asset;
using Flux.Asset.AssetSources;
using Flux.Asset.Interface;
using TestApp;

await using var catalogueFile = File.OpenRead("AssetRegistry.json");
await using var catalogueFileNewer = File.OpenRead("AssetRegistryNewer.json");

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

var cities = await assetService.Load<JsonAsset>(new Guid("f053d056-8ec2-42c2-a4ed-350934ad9f2e"));

Console.WriteLine(cities!.Root);