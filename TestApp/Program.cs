using Flux.Asset;
using Flux.Asset.AssetSource;
using TestApp;

await using var stream = File.OpenRead("AssetRegistry.json");

var registry = AssetCatalogue.Deserialize(stream);

var assetSource = new FileSystemAssetSource("Assets");
var assetService = new AssetsService(registry!, assetSource);
assetService.RegisterImporter<JsonAsset>(new JsonImporter());

var cities = await assetService.Load<JsonAsset>(new Guid("f053d056-8ec2-42c2-a4ed-350934ad9f2e"));

Console.WriteLine(cities!.Root);

