using DefaultEcs.System;
using Flux.Abstraction;
using Flux.Ecs;
using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.OpenGL.Extensions.ImGui;

namespace Flux.ImGuiFlux;

public class ImGuiRenderSystem : AComponentSystem<float, IUIRenderComponent>
{
    readonly ImGuiController imGui;

    bool showDemoSystem;

    public ImGuiRenderSystem(IEcsWorldService ecsService, ImGuiController imGui, IInputContext input)
        : base(ecsService.World)
    {
        this.imGui = imGui;

        foreach (var keyboard in input.Keyboards)
        {
            keyboard.KeyDown += KeyDown;
        }

        ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;
    }

    void KeyDown(IKeyboard keyboard, Key key, int arg)
    {
        if (key == Key.F1)
            showDemoSystem = !showDemoSystem;
    }

    protected override void PreUpdate(float deltatime)
    {
        base.PreUpdate(deltatime);
        imGui.Update(deltatime);
        
        ImGui.DockSpaceOverViewport(0u, ImGui.GetMainViewport(), ImGuiDockNodeFlags.PassthruCentralNode | ImGuiDockNodeFlags.AutoHideTabBar);

        if (showDemoSystem)
            ImGui.ShowDemoWindow();
    }

    protected override void Update(float deltatime, ref IUIRenderComponent component)
    {
        base.Update(deltatime, ref component);
        component.RenderUI(deltatime);
    }

    protected override void PostUpdate(float deltatime)
    {
        base.PostUpdate(deltatime);
        imGui.Render();
    }
}
