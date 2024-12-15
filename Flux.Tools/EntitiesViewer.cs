using DefaultEcs;
using Flux.Ecs;
using Flux.EntityBehavior;
using ImGuiNET;

namespace Flux.Tools;

public class EntitiesViewer : Behavior, IUIDrawable
{
    readonly EntitySet allEntitiesSet;
    
    public EntitiesViewer(IEcsWorldService ecsWorld)
    {
        allEntitiesSet = ecsWorld.World.GetEntities()
            .AsSet();
    }

    public void DrawUI(float deltatime)
    {
        if (ImGui.Begin("Entities viewer"))
        {
            const ImGuiTableFlags tableFlags =
                ImGuiTableFlags.Hideable
                | ImGuiTableFlags.Sortable
                | ImGuiTableFlags.SortMulti
                | ImGuiTableFlags.RowBg
                | ImGuiTableFlags.BordersV
                | ImGuiTableFlags.NoBordersInBody
                | ImGuiTableFlags.ScrollY;

            if (ImGui.BeginTable("Entities viewer", 1, tableFlags))
            {
                var sortsSpecs = ImGui.TableGetSortSpecs();
                if (sortsSpecs.SpecsDirty)
                {
                    sortsSpecs.SpecsDirty = false;
                }

                var clipper = new ImGuiListClipper();
                ImGuiListClipperPtr clipperPtr;
                unsafe
                {
                    clipperPtr = new ImGuiListClipperPtr(&clipper);
                }

                clipperPtr.Begin(allEntitiesSet.Count);
                {
                    while (clipperPtr.Step())
                    {
                        DisplayEntities(clipperPtr);
                    }
                }
                clipperPtr.End();
            }
            ImGui.EndTable();
        }
        ImGui.End();
    }
    void DisplayEntities(ImGuiListClipperPtr clipperPtr)
    {
        for (var rowN = clipperPtr.DisplayStart; rowN < clipperPtr.DisplayEnd; rowN++)
        {
            var entity = allEntitiesSet.GetEntities()[rowN];

            var isSelected = entity.Has<Selected>();

            var name = "Unnamed";
            if (entity.Has<string>())
                name = entity.Get<string>();

            ImGui.PushID(name);
            {
                ImGui.TableNextRow();
                ImGui.TableNextColumn();
                if (ImGui.Selectable(name, isSelected))
                {
                    if (isSelected)
                        entity.Remove<Selected>();
                    else
                        entity.Set<Selected>();
                }
            }
            ImGui.PopID();
        }
    }
}
