using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.OpenGL.Extensions.ImGui;

namespace Flux.ImGuiFlux;

public class ImguiService
{
    readonly ImGuiController imGui;

    public bool ShowDemoSystem { get; private set; }
    public bool InteractionEnabled { get; private set; } = true;

    public ImguiService(ImGuiController imGui)
    {
        this.imGui = imGui;

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

    public void Update(float deltatime) => imGui.Update(deltatime);
    public void Render() => imGui.Render();
}