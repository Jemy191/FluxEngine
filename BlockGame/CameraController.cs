using System.Numerics;
using Flux.EntityBehavior;
using Flux.EntityBehavior.Interfaces;
using Flux.MathAddon;
using Flux.Rendering;
using ImGuiNET;
using JetBrains.Annotations;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace BlockGame;

class CameraController : Behavior, IUpdatable, IUIDrawable
{
    [PublicAPI]
    public float LookSensitivity { get; set; } = 0.1f;
    [PublicAPI]
    public float MoveSpeed { get; set; } = 7f;

    Vector2 lastMousePosition;
    bool cameraLookActive;

    readonly IInputContext input;
    readonly IKeyboard primaryKeyboard;
    readonly IMouse primaryMouse;

    Vector2 rotation;

    public CameraController(IInputContext input, IWindow window)
    {
        this.input = input;
        primaryKeyboard = input.Keyboards[0];
        primaryMouse = input.Mice[0];

        foreach (var keyboard in input.Keyboards)
        {
            keyboard.KeyDown += KeyDown;
        }

        window.Resize += OnResize;

        ToggleLookAround();
    }

    void OnResize(Vector2D<int> size) => GetComponent<Camera>().aspectRatio = size.X / (float)size.Y;

    void KeyDown(IKeyboard keyboard, Key key, int arg)
    {
        if (key == Key.Tab)
            ToggleLookAround();
    }

    void ToggleLookAround()
    {
        lastMousePosition = primaryMouse.Position;

        cameraLookActive = !cameraLookActive;
        foreach (var mouse in input.Mice)
        {
            mouse.Cursor.CursorMode = cameraLookActive ? CursorMode.Raw : CursorMode.Normal;
        }
    }

    public void Update(float deltatime)
    {
        ref var transform = ref GetComponent<Transform>();
        transform.Position += Move(transform, deltatime);
        transform.Rotation = Look(transform, deltatime);
    }

    public void DrawUI(float deltatime)
    {
        ImGui.Begin("Camera");
        {
            var newMoveSpeed = MoveSpeed;
            ImGui.SliderFloat("Speed", ref newMoveSpeed, 0.5f, 200f);
            MoveSpeed = newMoveSpeed;
        }
        ImGui.End();
    }

    Vector3 Move(Transform transform, float deltatime)
    {
        var forward = transform.Forward;
        var up = transform.Up;
        var right = transform.Right;

        var direction = Vector3.Zero;

        if (primaryKeyboard.IsKeyPressed(Key.W))
            direction += forward;
        if (primaryKeyboard.IsKeyPressed(Key.S))
            direction -= forward;

        if (primaryKeyboard.IsKeyPressed(Key.A))
            direction -= right;
        if (primaryKeyboard.IsKeyPressed(Key.D))
            direction += right;

        if (primaryKeyboard.IsKeyPressed(Key.Space))
            direction += up;
        if (primaryKeyboard.IsKeyPressed(Key.ShiftLeft))
            direction -= up;

        if (direction == Vector3.Zero)
            return direction;
        return Vector3.Normalize(direction) * MoveSpeed * deltatime;
    }
    Quaternion Look(Transform transform, float deltatime)
    {
        if (!cameraLookActive)
            return transform.Rotation;

        var position = primaryMouse.Position;

        if (lastMousePosition == default)
        {
            lastMousePosition = position;
            return transform.Rotation;
        }

        var mouseDelta = position - lastMousePosition;

        rotation += mouseDelta * LookSensitivity * deltatime;

        lastMousePosition = position;

        var max = Angle.FromDegrees(90).Radians;
        rotation.Y = Math.Clamp(rotation.Y, -max, max);

        return Quaternion.CreateFromYawPitchRoll(rotation.X, rotation.Y, 0);
    }
}
