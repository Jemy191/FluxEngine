using System.Numerics;
using Flux.EntityBehavior;
using Flux.MathAddon;
using Flux.Rendering;
using Flux.Rendering.Extensions;
using Flux.Rendering.Services;
using Flux.Tools;
using Silk.NET.Input;
using Silk.NET.Windowing;

namespace TestApp;

class Game
{
    readonly IWindow window;

    public Game(IInputContext input, IWindow window, BehaviorService behaviorService, ModelEntityBuilderService modelBuilder)
    {
        this.window = window;
        foreach (var keyboard in input.Keyboards)
        {
            keyboard.KeyDown += KeyDown;
        }

        CreateCamera(window, behaviorService);

        modelBuilder
            .Name("Suzane")
            .Vertex("shader.vert".ToAsset())
            .Fragment("suzane.frag".ToAsset())
            .Model("Suzane.fbx".ToAsset())
            .Position(new Vector3(0, 5, 0))
            .Create();

        modelBuilder
            .Name("Cube")
            .Vertex("shader.vert".ToAsset())
            .Fragment("normal.frag".ToAsset())
            .Model("Cube.fbx".ToAsset())
            .Texture("albedo", "BrickPBR/Brick_albedo.png".ToAsset())
            .Texture("normal", "BrickPBR/Brick_normal.png".ToAsset())
            .Position(new Vector3(0, 0.5f, 0))
            .Create();

        modelBuilder
            .Name("Terrain")
            .Fragment("lighting.frag".ToAsset())
            .Model("Terrain.fbx".ToAsset())
            .Texture("albedo", "Terrain.png".ToAsset())
            .RemoveTexture("normal")
            .Position(Vector3.Zero)
            .Scale(Vector3.One * 5f)
            .Create();

        var (_, behavior) = behaviorService.CreateBehaviorEntity();
        behavior.AddBehavior<EntitiesViewer>();
        behavior.AddBehavior<EntitiesInspector>();
    }

    static void CreateCamera(IWindow window, BehaviorService behaviorService)
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