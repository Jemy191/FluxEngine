using Flux.Asset;
using Flux.Asset.AssetSources;
using Flux.Asset.Interface;
using Flux.Engine;
using Flux.Engine.AssetImporters;
using Flux.Engine.Assets;
using Flux.EntityBehavior;
using Flux.ImGuiFlux;
using Flux.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Silk.NET.Windowing;
using TestApp;

var builder = new GameEngineBuilder("Test engine");

// Add services here

AssetCatalogue assetsCatalogue;
List<AssetSource> assetSources = [];
using (var assetsCatalogueFile = File.OpenRead("AssetsCatalogue.json"))
{
    assetsCatalogue = new AssetCatalogue(assetsCatalogueFile, new Dictionary<string, Type>(StringComparer.Ordinal) { { "Path", typeof(string) } });
    assetSources.Add(new FileSystemAssetSource(assetsCatalogue, "Assets"));
}
using (var assetsCatalogueFile = File.OpenRead("ModsCatalogue.json"))
{
    assetsCatalogue = new AssetCatalogue(assetsCatalogueFile, new Dictionary<string, Type>(StringComparer.Ordinal) { { "Path", typeof(string) } });
    assetSources.Add(new FileSystemAssetSource(assetsCatalogue, "Mods"));
}

var assetsService = new AssetsService(assetSources);
assetsService.RegisterImporter<MeshAsset, GltfImporter>();
assetsService.RegisterImporter<TextureAsset, TextureImporter>();

builder.Services
    .AddSilkInput()
    .AddOpenGL<IWindow>()
    .AddImGui()
    .AddResourceServices()
    .AddBehaviorServices()
    .AddModelEntityBuilder()
    .AddSingleton(assetsService);

var engine = builder.Build();

// Add render or update system here

engine.AddOpenGlRendering()
    .AddModelRendering()
    .AddImGuiRendering()
    .AddBehaviorSystem();

// Add game logic here

engine.RunWith<Game>();
