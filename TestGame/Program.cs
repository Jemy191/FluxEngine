using Flux.Asset;
using Flux.Asset.Utils;
using Flux.Engine;
using Flux.EntityBehavior;
using Flux.ImGuiFlux;
using Flux.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Silk.NET.Windowing;
using TestApp;

var builder = new GameEngineBuilder("Test engine");

// Add services here

var assetCatalogue = AssetCatalogueBuilder.BuildFromDirectory("Assets");

File.WriteAllText(@"C:\Users\Emilie\Documents\FluxEngine\TestGame\AssetCatalogue.json", assetCatalogue.Serialize());

builder.Services
    .AddSilkInput()
    .AddOpenGL<IWindow>()
    .AddImGui()
    .AddResourceServices()
    .AddBehaviorServices()
    .AddModelEntityBuilder();

var engine = builder.Build();

// Add render or update system here

engine.AddOpenGlRendering()
    .AddModelRendering()
    .AddImGuiRendering()
    .AddBehaviorSystem();

// Add game logic here

engine.RunWith<Game>();