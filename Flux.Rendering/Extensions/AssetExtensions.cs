using Flux.Core;

namespace Flux.Rendering.Extensions;

public static class AssetExtensions
{
    extension(string filePath)
    {
        public FileInfo ToAsset() => AssetGlobalSettings.AssetsPath.ToFile(filePath);
        public FileInfo ToEngineInternalAsset() => AssetGlobalSettings.EngineInternalAssetPath.ToFile(filePath);
    }
}