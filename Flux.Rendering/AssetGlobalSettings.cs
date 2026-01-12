using Flux.Core;

namespace Flux.Rendering;

public static class AssetGlobalSettings
{
    public static DirectoryInfo AssetsPath { get; set; } = "Assets".ToDirectory();
    public static DirectoryInfo EngineInternalAssetPath { get; set; } = Path.Join("Assets", "EngineInternal").ToDirectory();

    public static void Initialize(DirectoryInfo assetsDir)
    {
        AssetsPath = assetsDir;
        EngineInternalAssetPath = Path.Join(assetsDir.FullName, "EngineInternal").ToDirectory();
    }
}