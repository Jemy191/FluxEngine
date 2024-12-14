using BlockGame;
using Flux.Asset;
using Flux.Asset.AssetSources;
using Flux.Engine;
using Flux.Engine.AssetImporters;
using Flux.EntityBehavior;
using Flux.ImGuiFlux;
using Flux.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Silk.NET.Windowing;

var builder = new GameEngineBuilder("Test engine");

// Add services here

AssetCatalogue assetsCatalogue;
List<AssetSource> assetSources = [];
using (var assetsCatalogueFile = File.OpenRead("AssetsCatalogue.json"))
{
    assetsCatalogue = new AssetCatalogue(assetsCatalogueFile, new Dictionary<string, Type>(StringComparer.Ordinal) { { "Path", typeof(string) } });
    assetSources.Add(new FileSystemAssetSource(assetsCatalogue, "Assets"));
}

var assetsService = new AssetsService(assetSources);


builder.Services
    .AddSilkInput()
    .AddOpenGL<IWindow>()
    .AddImGui()
    .AddBehaviorServices()
    .AddModelEntityBuilder()
    .AddSingleton(assetsService);

var engine = builder.Build();

// Add render or update system here

engine.AddOpenGlRendering()
    .AddModelRendering()
    .AddImGuiRendering()
    .AddBehaviorSystem();

assetsService.RegisterImporter<GltfImporter>();
assetsService.RegisterImporter<TextureImporter>();
assetsService.RegisterImporter<ShaderImporter>();
assetsService.RegisterImporter<JsonImporter>();
var resourceImporter = engine.Instantiate<ResourceImporter<Game>>();
assetsService.RegisterImporter(resourceImporter);

// Add game logic here

engine.RunWith<Game>();