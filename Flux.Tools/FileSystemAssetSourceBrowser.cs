using Flux.Asset;
using Flux.Asset.AssetSources;
using ImGuiNET;

namespace Flux.Tools;

public class FileSystemAssetSourceBrowser : IAssetSourceBrowser<FileSystemAssetSource>
{
    public void Draw(FileSystemAssetSource source)
    {
        DrawAssetTree(source.AssetTree);
    }

    void DrawAssetTree(AssetTree tree)
    {
        var baseFlag =
            ImGuiTreeNodeFlags.SpanAvailWidth;// | ImGuiTreeNodeFlags.Selected;

        if (tree.CatalogueAsset is not null)
            baseFlag |= ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.NoTreePushOnOpen;


        var nodeOpen = ImGui.TreeNodeEx(tree.Name, baseFlag);

        if (ImGui.IsItemClicked() && !ImGui.IsItemToggledOpen())
            Console.WriteLine("Clicked");

        if (ImGui.BeginDragDropSource())
        {
            ImGui.SetDragDropPayload(tree.Name, IntPtr.Zero, 0);
            ImGui.Text(tree.Name);
            ImGui.EndDragDropSource();
        }

        if (tree.CatalogueAsset is not null)
            return;
        
        if (nodeOpen)
        {
            foreach (var child in tree.Children.Values)
            {
                DrawAssetTree(child);
            }
            ImGui.TreePop();
        }
    }
}