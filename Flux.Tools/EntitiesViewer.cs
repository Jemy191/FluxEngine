using DefaultEcs;
using Flux.Ecs;
using Flux.EntityBehavior;
using ImGuiNET;

namespace Flux.Tools;

public class EntitiesViewer : Behavior, IUIDrawable
{
    readonly IEcsWorldService ecsWorld;
    readonly EntitySet allEntitiesSet;

    Entity? selectedEntity = null;

    public EntitiesViewer(IEcsWorldService ecsWorld)
    {
        this.ecsWorld = ecsWorld;
        allEntitiesSet = ecsWorld.World.GetEntities()
            .AsSet();
    }

    public void DrawUI(float deltatime)
    {
        if (ImGui.Begin("Entities viewer"))
        {
            ImGuiTableFlags tableFlags =
                ImGuiTableFlags.Hideable
                | ImGuiTableFlags.Sortable
                | ImGuiTableFlags.SortMulti
                | ImGuiTableFlags.RowBg
                | ImGuiTableFlags.BordersOuter
                | ImGuiTableFlags.BordersV
                | ImGuiTableFlags.NoBordersInBody
                | ImGuiTableFlags.ScrollY;

            if (ImGui.BeginTable("Entities viewer", 1, tableFlags))
            {
                var sorts_specs = ImGui.TableGetSortSpecs();
                if (sorts_specs.SpecsDirty)
                {
                    //MyItem::s_current_sort_specs = sorts_specs; // Store in variable accessible by the sort function.
                    //if (items.Size > 1)
                    //    qsort(&items[0], (size_t)items.Size, sizeof(items[0]), MyItem::CompareWithSortSpecs);
                    //MyItem::s_current_sort_specs = NULL;
                    sorts_specs.SpecsDirty = false;
                }

                var t = new ImGuiListClipper();
                ImGuiListClipperPtr clipper;
                unsafe
                {
                    clipper = new ImGuiListClipperPtr(&t);
                }

                clipper.Begin(allEntitiesSet.Count);
                {
                    while (clipper.Step())
                    {
                        for (int row_n = clipper.DisplayStart; row_n < clipper.DisplayEnd; row_n++)
                        {
                            var entity = allEntitiesSet.GetEntities()[row_n];

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
                clipper.End();
            }
            ImGui.EndTable();
        }
        ImGui.End();
    }

    /*static int IMGUI_CDECL CompareWithSortSpecs(const void* lhs, const void* rhs)
    {
        const MyItem* a = (const MyItem*)lhs;
        const MyItem* b = (const MyItem*)rhs;
        for (int n = 0; n<s_current_sort_specs->SpecsCount; n++)
        {
            // Here we identify columns using the ColumnUserID value that we ourselves passed to TableSetupColumn()
            // We could also choose to identify columns based on their index (sort_spec->ColumnIndex), which is simpler!
            const ImGuiTableColumnSortSpecs* sort_spec = &s_current_sort_specs->Specs[n];
    int delta = 0;
            switch (sort_spec->ColumnUserID)
            {
            case MyItemColumnID_ID:             delta = (a->ID - b->ID);                break;
            case MyItemColumnID_Name:           delta = (strcmp(a->Name, b->Name));     break;
            case MyItemColumnID_Quantity:       delta = (a->Quantity - b->Quantity);    break;
            case MyItemColumnID_Description:    delta = (strcmp(a->Name, b->Name));     break;
            default: IM_ASSERT(0); break;
            }
            if (delta > 0)
                return (sort_spec->SortDirection == ImGuiSortDirection_Ascending) ? +1 : -1;
            if (delta< 0)
                return (sort_spec->SortDirection == ImGuiSortDirection_Ascending) ? -1 : +1;
        }

        // qsort() is instable so always return a way to differenciate items.
        // Your own compare function may want to avoid fallback on implicit sort specs e.g. a Name compare if it wasn't already part of the sort specs.
        return (a->ID - b->ID);
    }*/
}
