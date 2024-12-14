using System.Numerics;
using Flux.Abstraction;
using Flux.EntityBehavior;
using Flux.MathAddon;
using Flux.Rendering;
using Silk.NET.Input;
using Silk.NET.Windowing;

namespace BlockGame;

class Game : IGame
{
    readonly IInputContext input;
    readonly IWindow window;
    readonly BehaviorService behaviorService;

    public Game(IInputContext input, IWindow window, BehaviorService behaviorService)
    {
        this.input = input;
        this.window = window;
        this.behaviorService = behaviorService;
    }
    public async Task Initialize()
    {
        foreach (var keyboard in input.Keyboards)
        {
            keyboard.KeyDown += KeyDown;
        }

        await CreateCamera(window, behaviorService);
        await behaviorService.CreateSingleBehaviorEntity<BlockControl>();
    }

    static async Task CreateCamera(IWindow window, BehaviorService behaviorService)
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

        await cameraBehavior.AddBehavior<CameraController>();
    }

    void KeyDown(IKeyboard keyboard, Key key, int arg)
    {
        switch (key)
        {
            case Key.Escape: window.Close();
                break;
            case Key.F2 when window.WindowState == WindowState.Fullscreen: window.WindowState = WindowState.Maximized;
                break;
            case Key.F2:
                window.WindowState = WindowState.Normal;
                window.WindowState = WindowState.Fullscreen;
                break;
        }
    }
}