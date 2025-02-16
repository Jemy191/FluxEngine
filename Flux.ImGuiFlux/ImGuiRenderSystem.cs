using DefaultEcs.System;
using Flux.Abstraction;
using Flux.Ecs;
using ImGuiNET;

namespace Flux.ImGuiFlux;

public class ImGuiRenderSystem : AComponentSystem<float, IUIRenderComponent>
{
    readonly ImguiService imGui;

    public ImGuiRenderSystem(IEcsWorldService ecsService, ImguiService imGui)
        : base(ecsService.World)
    {
        this.imGui = imGui;
    }

    protected override void PreUpdate(float deltatime)
    {
        imGui.Update(deltatime);

        //ImGui.DockSpaceOverViewport(ImGui.GetMainViewport(), ImGuiDockNodeFlags.PassthruCentralNode | ImGuiDockNodeFlags.AutoHideTabBar);

        if (imGui.ShowDemoSystem)
            ImGui.ShowDemoWindow();
    }

    protected override void Update(float deltatime, ref IUIRenderComponent component) => component.RenderUI(deltatime);

    protected override void PostUpdate(float deltatime) => imGui.Render();
}
