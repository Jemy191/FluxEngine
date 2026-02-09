using Flux.Assets.Interfaces;

namespace Flux.Assets;

public class AssetHandle<TAsset> : IAssetHandleInternal
{
    readonly AssetInfo assetInfo;
    readonly AssetManager manager;
    uint refCount = 1;

    public event Action? OnRefresh;

    public TAsset Asset { get; private set; }

    public AssetHandle(TAsset asset, AssetInfo assetInfo, AssetManager manager)
    {
        this.assetInfo = assetInfo;
        this.manager = manager;
        Asset = asset;
    }

    /// <summary> Replace the old resource. </summary>
    /// <remarks>Will call <see cref="OnRefresh"/></remarks>
    public void Refresh(TAsset asset)
    {
        Asset = asset;
        OnRefresh?.Invoke();
    }

    void IAssetHandleInternal.AddRef() => refCount++;

    public void Dispose()
    {
        refCount--;
        if (refCount == 0)
            manager.Unload(assetInfo);
    }

    public static implicit operator TAsset(AssetHandle<TAsset> handle) => handle.Asset;
}