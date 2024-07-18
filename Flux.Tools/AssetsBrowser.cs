using System.Numerics;
using Flux.Asset;
using Flux.EntityBehavior;
using Flux.EntityBehavior.Interfaces;
using ImGuiNET;

namespace Flux.Tools;

public class AssetsBrowser : Behavior, IUIDrawable
{
    readonly AssetsService assetsService;
    readonly HashSet<AssetSource> selectedSources = [];
    readonly Dictionary<Type, IAssetSourceBrowser> sourceBrowsers = [];

    public AssetsBrowser(AssetsService assetsService)
    {
        this.assetsService = assetsService;
    }

    public void RegisterAssetSourceBrowser<TBrowser, TSource>() where TBrowser : IAssetSourceBrowser<TSource>, new() where TSource : AssetSource =>
        sourceBrowsers.Add(typeof(TSource), new TBrowser());

    public void DrawUI(float deltatime)
    {
        if (ImGui.Begin("Assets browser"))
        {
            foreach (var assetSourcesByType in assetsService.AssetSources.GroupBy(s => s.GetType()))
            {
                ImGui.BeginTabBar("Asset source type");
                {
                    ImGui.BeginTabItem(assetSourcesByType.Key.Name.Replace("AssetSource", ""));
                    {
                        ImGui.BeginChild("Source selector", new Vector2(200, 0), true);
                        {
                            ImGui.Text("Source selector");
                            foreach (var assetSource in assetSourcesByType)
                            {
                                var selected = selectedSources.Contains(assetSource);
                                var newSelected = ImGui.Selectable(assetSource.Name, selected);

                                if (newSelected)
                                {
                                    if (selected)
                                        selectedSources.Remove(assetSource);
                                    else
                                        selectedSources.Add(assetSource);
                                }
                            }
                        }
                        ImGui.EndChild();
                        ImGui.SameLine();
                        ImGui.BeginChild("Asset view", Vector2.Zero, true);
                        {
                            foreach (var selectedSource in selectedSources)
                            {
                                sourceBrowsers[assetSourcesByType.Key].Draw(selectedSource);
                            }
                        }
                        ImGui.EndChild();
                    }
                    ImGui.EndTabItem();
                }
                ImGui.EndTabBar();
            }
        }
        ImGui.End();

    }
}