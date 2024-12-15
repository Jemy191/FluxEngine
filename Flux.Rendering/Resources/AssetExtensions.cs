using Flux.Core;

namespace Flux.Rendering.Resources;

public static class AssetExtensions
{
    public static FileInfo ToAsset(this string filePath) => "Assets".ToDirectory().ToFile(filePath);
}