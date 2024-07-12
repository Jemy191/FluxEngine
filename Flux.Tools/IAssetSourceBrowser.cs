using Flux.Asset;

namespace Flux.Tools;

public interface IAssetSourceBrowser<in T> : IAssetSourceBrowser where T : AssetSource
{
    void Draw(T source);

    void IAssetSourceBrowser.Draw(AssetSource source)
    {
        Draw((T)source);
    }
}

public interface IAssetSourceBrowser
{
    void Draw(AssetSource source);
}