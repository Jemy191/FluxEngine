using Flux.Core;

namespace Flux.Rendering;

public static class AssetGlobalSettings
{
    public static DirectoryInfo AssetsPath { get; set; } = "Assets".ToDirectory();
}