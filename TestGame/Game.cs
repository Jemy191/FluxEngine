using System.Numerics;
using Flux.Abstraction;
using Flux.Asset;
using Flux.Asset.AssetSources;
using Flux.Engine.Assets;
using Flux.EntityBehavior;
using Flux.MathAddon;
using Flux.Rendering;
using Flux.Tools;
using Silk.NET.Input;
using Silk.NET.Windowing;

namespace TestApp;

class Game : IGame
{
    readonly IInputContext input;
    readonly IWindow window;
    readonly BehaviorService behaviorService;
    readonly ModelEntityBuilderService modelBuilder;
    readonly AssetsService assetsService;

    public Game(IInputContext input, IWindow window, BehaviorService behaviorService, ModelEntityBuilderService modelBuilder, AssetsService assetsService)
    {
        this.input = input;
        this.window = window;
        this.behaviorService = behaviorService;
        this.modelBuilder = modelBuilder;
        this.assetsService = assetsService;
    }
    
    public async Task Initialize()
    {
        for (int i = 0; i < input.Keyboards.Count; i++)
        {
            input.Keyboards[i].KeyDown += KeyDown;
        }

        CreateCamera(window, behaviorService);

        var cubeMesh = await assetsService.Load<MeshAsset>(Guid.Parse("02f6ba34-d7d9-4e3f-a0f1-0c5e160e4a10"));
        var suzaneMesh = await assetsService.Load<MeshAsset>(Guid.Parse("6f116dea-6c4c-4842-a6ba-ed7451707e50"));
        var terrainMesh = await assetsService.Load<MeshAsset>(Guid.Parse("9b23f7ca-3b53-44c9-917d-1edb061b3edf"));
        var testMesh = await assetsService.Load<MeshAsset>(Guid.Parse("d7355025-3f08-4d52-86a4-31080fd07461"));
        
       var brickAlbedo = await assetsService.Load<TextureAsset>(Guid.Parse("c8e2e906-734b-4125-9d45-9d57e782e6ce"));
       var brickNormal = await assetsService.Load<TextureAsset>(Guid.Parse("5cfa7987-12f5-4e71-8a19-cb1fb41ddcc2"));
       var terrainTexture = await assetsService.Load<TextureAsset>(Guid.Parse("14c5bf87-6d37-40b2-91aa-c84d73ed2a35"));
       
       var simpleVertexShader = await assetsService.Load<ShaderAsset>(Guid.Parse("9615bc02-7461-45c2-9226-86c2d14337a4"));
       var suzaneFragmentShader = await assetsService.Load<ShaderAsset>(Guid.Parse("278fcd7d-bac0-4e2d-b475-9f980a67e239"));
       var normalMapFragmentShader = await assetsService.Load<ShaderAsset>(Guid.Parse("ece0ca35-9c07-4186-824f-e157e0449be7"));
       var lightingFragmentShader = await assetsService.Load<ShaderAsset>(Guid.Parse("270e25a4-36b9-4348-af14-534381740eb0"));
        
       modelBuilder
            .Name("Suzane")
            .Shader(simpleVertexShader)
            .Shader(suzaneFragmentShader)
            .Mesh(suzaneMesh!)
            .Position(new Vector3(0, 5, 0))
            .Create();

       
        modelBuilder
            .Name("Cube")
            .Shader(normalMapFragmentShader)
            .Mesh(cubeMesh!)
            .Texture("albedo", brickAlbedo)
            .Texture("normal", brickNormal)
            .Position(new Vector3(0, 0.5f, 0))
            .Scale(Vector3.One)
            .Create();
        
        modelBuilder
            .Name("Terrain")
            .Shader(lightingFragmentShader)
            .Mesh(terrainMesh!)
            .Texture("albedo", terrainTexture)
            .RemoveTexture("normal")
            .Position(Vector3.Zero)
            .Scale(Vector3.One * 5f)
            .Create();

        /*modelBuilder
            .SetName("Axis")
            .SetVertex("Axis.vert")
            .SetFragment("Axis.frag")
            .SetMesh("Axis.fbx")
            .RemoveTexture("albedo")
            .RemoveTexture("normal")
            .SetPosition(Vector3.Zero)
            .SetScale(Vector3.One)
            .Create();*/

        var (entity, behavior) = behaviorService.CreateBehaviorEntity();
        // Crash the app
        //behavior.AddBehavior<EntitiesViewer>();
        // Not needed
        //behavior.AddBehavior<EntitiesInspector>();
        behavior.AddBehavior<AssetsBrowser>();
        var assetBrowser = behavior.GetBehavior<AssetsBrowser>();
        assetBrowser.RegisterAssetSourceBrowser<FileSystemAssetSourceBrowser, FileSystemAssetSource>();
    }

    void CreateCamera(IWindow window, BehaviorService behaviorService)
    {
        // Camera
        var viewportSize = window.Size;
        var cam = new Camera
        {
            aspectRatio = viewportSize.X / (float)viewportSize.Y,
            fov = Angle.FromDegrees(60f)
        };

        var (camera, cameraBehavior) = behaviorService.CreateBehaviorEntity();

        var transform = new Transform
        {
            Position = new Vector3(0, 5, -10)
        };
        camera.Set("Camera");
        camera.Set(transform);
        camera.Set(cam);

        cameraBehavior.AddBehavior<CameraController>();
    }

    void KeyDown(IKeyboard keyboard, Key key, int arg)
    {
        if (key == Key.Escape)
            window.Close();
        if (key == Key.F2)
        {
            if (window.WindowState == WindowState.Fullscreen)
            {
                window.WindowState = WindowState.Maximized;
            }
            else
            {
                window.WindowState = WindowState.Normal;
                window.WindowState = WindowState.Fullscreen;
            }
        }
    }
}