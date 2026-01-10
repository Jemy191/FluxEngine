using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;

namespace Flux.ImGuiFlux;

public class ImguiService
{
    readonly ImGuiController imGui;

    public bool ShowDemoSystem { get; private set; }
    public bool InteractionEnabled { get; private set; } = true;
    public ImGuiDockNodeFlags DockSpaceFlags { get; set; } = ImGuiDockNodeFlags.PassthruCentralNode | ImGuiDockNodeFlags.AutoHideTabBar | ImGuiDockNodeFlags.NoDockingOverCentralNode;

    public ImguiService(GL gl, IWindow window, IInputContext input)
    {
        imGui = new ImGuiController(gl, window, input, () =>
        {
            ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;
        });
    }

    public void ToggleInteraction(bool enable)
    {
        InteractionEnabled = enable;
        if (InteractionEnabled)
            ImGui.GetIO().ConfigFlags &= ~(ImGuiConfigFlags.NoMouse | ImGuiConfigFlags.NavNoCaptureKeyboard);
        else
            ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.NoMouse | ImGuiConfigFlags.NavNoCaptureKeyboard;
    }

    public void Update(float deltatime) => imGui.Update(deltatime);
    public void Render() => imGui.Render();
}