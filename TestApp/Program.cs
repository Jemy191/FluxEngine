using Flux.Engine;
using Flux.EntityBehavior;
using Flux.ImGuiFlux;
using Flux.Rendering.Extensions;
using Flux.Resources;
using Silk.NET.Windowing;
using TestApp;

var builder = new GameEngineBuilder("Test engine");

// Add services here

builder.Services
    .AddSilkInput()
    .AddOpenGL<IWindow>()
    .AddImGui()
    .AddLoaderServices()
    .AddBehaviorServices()
    .AddModelEntityBuilder()
    .AddResourcesManagement()
    ;

var engine = builder.Build();

// Add render or update system here

engine.AddStartOfFrameOpenGlRendering()
    .AddModelRendering()
    .AddImGuiRendering()
    .AddBehaviorSystem()
    .AddResourceManagers()
    ;

// Add game logic here

engine.RunWith<Game>();