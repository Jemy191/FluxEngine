using Flux.Asset;
using Flux.Asset.AssetSources;
using Flux.Engine;
using Flux.Engine.AssetImporters;
using Flux.Engine.AssetInterfaces;
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

var assetsService = new AssetsService(assetSources);
assetsService.RegisterImporter<IMeshAsset, GltfImporter>();
assetsService.RegisterImporter<ITextureAsset, TextureImporter>();
assetsService.RegisterImporter<IShaderAsset, ShaderImporter>();
assetsService.RegisterImporter<IJsonAsset, JsonImporter>();

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

// Add game logic here

engine.RunWith<Game>();
