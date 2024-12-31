using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.OpenGL.Extensions.ImGui;

namespace Flux.ImGuiFlux;

public class ImguiService
{
    readonly ImGuiController imGui;

    public bool ShowDemoSystem { get; private set; }
    public bool InteractionEnabled { get; private set; } = true;

    public ImguiService(ImGuiController imGui, IInputContext input)
    {
        this.imGui = imGui;

        foreach (var keyboard in input.Keyboards)
        {
            keyboard.KeyDown += KeyDown;
        }
        ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;
    }

    public void ToggleInteraction(bool enable)
    {
        InteractionEnabled = enable;
        if (InteractionEnabled)
            ImGui.GetIO().ConfigFlags &= ~(ImGuiConfigFlags.NoMouse | ImGuiConfigFlags.NavNoCaptureKeyboard);
        else
            ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.NoMouse | ImGuiConfigFlags.NavNoCaptureKeyboard;
    }

    void KeyDown(IKeyboard keyboard, Key key, int arg)
    {
        if (key == Key.F1)
            ShowDemoSystem = !ShowDemoSystem;
    }

    public void Update(float deltatime) => imGui.Update(deltatime);
    public void Render() => imGui.Render();
}