using Flux.Asset;
using Flux.Asset.AssetSource;
using TestApp;

await using var catalogueFile = File.OpenRead("AssetRegistry.json");

var catalogue = new AssetCatalogue(catalogueFile, new Dictionary<string, Type>
{
    { "Path", typeof(string) }
});

var assetSource = new FileSystemAssetSource(catalogue, "Assets");
var assetService = new AssetsService(assetSource);
assetService.RegisterImporter<JsonAsset>(new JsonImporter());

var cities = await assetService.Load<JsonAsset>(new Guid("f053d056-8ec2-42c2-a4ed-350934ad9f2e"));

Console.WriteLine(cities!.Root);