using Flux.Core;

namespace Flux.Rendering.Extensions;

public static class AssetExtensions
{
    public static FileInfo ToAsset(this string filePath) => "Assets".ToDirectory().ToFile(filePath);
}