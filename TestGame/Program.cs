using Flux.Asset;
using Flux.Asset.AssetSources;
using Flux.Asset.Interface;
using Flux.Engine;
using Flux.EntityBehavior;
using Flux.ImGuiFlux;
using Flux.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Silk.NET.Windowing;
using TestApp;

var builder = new GameEngineBuilder("Test engine");

// Add services here

AssetsService assetsService;

using (var assetsCatalogueFile = File.OpenRead("AssetCatalogue.json"))
{
    var assetsCatalogue = new AssetCatalogue(assetsCatalogueFile, new Dictionary<string, Type>(StringComparer.Ordinal) { { "Path", typeof(string) } });
    List<AssetSource> assetSources = [new FileSystemAssetSource(assetsCatalogue, "Assets")];
    assetsService = new AssetsService(assetSources);
}

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
