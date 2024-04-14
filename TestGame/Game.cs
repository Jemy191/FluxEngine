using System.Numerics;
using Flux.Ecs;
using Flux.EntityBehavior;
using Flux.MathAddon;
using Flux.Rendering;
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
        for (int i = 0; i < input.Keyboards.Count; i++)
        {
            input.Keyboards[i].KeyDown += KeyDown;
        }

        CreateCamera(window, behaviorService);

        modelBuilder
            .Name("Suzane")
            .Vertex("shader.vert")
            .Fragment("suzane.frag")
            .Mesh("Suzane.fbx")
            .Position(new Vector3(0, 5, 0))
            .Create();

        modelBuilder
            .Name("Cube")
            .Vertex("shader.vert")
            .Fragment("normal.frag")
            .Mesh("Cube.fbx")
            .Texture("albedo", "BrickPBR/Brick_albedo.png")
            .Texture("normal", "BrickPBR/Brick_normal.png")
            .Position(new Vector3(0, 0.5f, 0))
            .Create();

        modelBuilder
            .Name("Terrain")
            .Fragment("lighting.frag")
            .Mesh("Terrain.fbx")
            .Texture("albedo", "Terrain.png")
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
        behavior.AddBehavior<EntitiesViewer>();
        behavior.AddBehavior<EntitiesInspector>();
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