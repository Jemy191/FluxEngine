using DefaultEcs;
using DefaultEcs.Serialization;
using Flux.Ecs;
using Flux.EntityBehavior;
using Flux.EntityBehavior.Interfaces;
using ImGuiNET;

namespace Flux.Tools;

public class EntitiesInspector : Behavior, IUIDrawable
{
    readonly IEcsWorldService ecsWorld;
    readonly EntitySet selectedEntitiesSet;

    public EntitiesInspector(IEcsWorldService ecsWorld)
    {
        this.ecsWorld = ecsWorld;
        selectedEntitiesSet = ecsWorld.World.GetEntities()
            .With<Selected>()
            .AsSet();
    }

    public void DrawUI(float deltatime)
    {
        if (ImGui.Begin("Inspector"))
        {
            foreach (var entity in selectedEntitiesSet.GetEntities())
            {
                var name = "Unnamed";
                if (entity.Has<string>())
                    name = entity.Get<string>();

                ImGui.Text(name);
                ImGui.Indent();
                {
                    entity.ReadAllComponents(new ComponentReader());
                }
                ImGui.Unindent();
                ImGui.Separator();
            }
        }
        ImGui.End();
    }

    class ComponentReader : IComponentReader
    {
        public void OnRead<T>(in T component, in Entity componentOwner)
        {
            if (component is null)
                return;

            switch (component)
            {
                case Selected:
                    break;
                default:
                    ImGui.Text(component.GetType().Name);
                    ImGui.Indent();
                    {
                        ImGui.Text(component.ToString());
                    }
                    ImGui.Unindent();
                    break;
            }
        }
    }
}
