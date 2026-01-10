using Flux.Abstraction;
using Flux.EntityBehavior;
using ImGuiNET;

namespace Flux.Tools
{
    public class SimpleUIWindowBehavior<T> : Behavior, IUIDrawable where T : IUIDrawable, INamedUI
    {
        readonly T ui;

        public SimpleUIWindowBehavior(IInjectionService injectionServices) => ui = injectionServices.Instantiate<T>();
        public SimpleUIWindowBehavior(T ui) => this.ui = ui;

        public void DrawUI(float deltatime)
        {
            ImGui.Begin(ui.Name);
            {
                ui.DrawUI(deltatime);
            }
            ImGui.End();
        }
    }

}