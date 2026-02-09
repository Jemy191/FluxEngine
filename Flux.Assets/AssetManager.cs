using System.Collections.Frozen;
using Flux.Assets.Exceptions;
using Flux.Assets.Interfaces;
using Flux.Core;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Flux.Assets;

/// <summary>
/// This class manages the loading and retrieval of assets.
/// To register loaders, you can use <see cref="Extensions.AddAssetLoader{TLoader, TAsset}"/>
/// </summary>
public class AssetManager
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public record AssetManagerOptions(DirectoryInfo AssetsPath)
    {
        public static readonly DirectoryInfo DefaultAssetsDirectory = "Assets".ToDirectory();
        public static readonly AssetManagerOptions Default = new AssetManagerOptions(DefaultAssetsDirectory);
    }

    readonly FrozenDictionary<FileExtension, IAssetLoader> loadersByExtension;
    readonly Dictionary<AssetInfo, IAssetHandleInternal> assetHandles = [];

    public readonly DirectoryInfo AssetsDirectory;
    public readonly DirectoryInfo EngineInternalAssetDirectory;

    public AssetManager(IEnumerable<IAssetLoader> loaders, IOptions<AssetManagerOptions> options)
    {
        AssetsDirectory = options.Value.AssetsPath;
        EngineInternalAssetDirectory = AssetsDirectory.Join("EngineInternal");

        Dictionary<FileExtension, IAssetLoader> byExtension = [];

        foreach (var loader in loaders)
        {
            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var supportedExtension in loader.SupportedExtensions)
            {
                if (!byExtension.TryAdd(supportedExtension, loader))
                    throw new AssetLoaderRegistrationException(supportedExtension, loader.GetType());
            }
        }

        loadersByExtension = byExtension.ToFrozenDictionary();
    }

    public FileInfo ResolvePath(AssetInfo assetInfo)
    {
        var currentAssetDirectory = assetInfo is EngineInternalAssetInfo
            ? EngineInternalAssetDirectory
            : AssetsDirectory;

        var fileInfo = currentAssetDirectory.ToFile(assetInfo.RelativePath);
        if (!fileInfo.Exists)
            throw new FileNotFoundException($"Asset file {assetInfo.RelativePath} does not exist");

        return fileInfo;
    }

    protected IAssetLoader GetLoader<T>(AssetInfo assetInfo)
    {
        if (!loadersByExtension.TryGetValue(assetInfo.Extension, out var loader)
            || !loader.IsTypeSupported<T>())
        {
            throw new AssetLoaderNotFoundException<T>(assetInfo);
        }

        return loader;
    }

    public AssetHandle<T> Load<T>(AssetInfo asset)
    {
        if (!assetHandles.TryGetValue(asset, out var handle))
        {
            var loader = GetLoader<T>(asset);
            var loadedAsset = loader.Load<T>(ResolvePath(asset));
            handle = new AssetHandle<T>((T)loadedAsset, asset, this);
            assetHandles.Add(asset, handle);
        }
        handle.AddRef();
        return (AssetHandle<T>)handle;
    }
    public void Unload(AssetInfo assetInfo) => assetHandles.Remove(assetInfo);
}