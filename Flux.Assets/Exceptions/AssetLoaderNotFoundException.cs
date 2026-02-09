namespace Flux.Assets.Exceptions;

public class AssetLoaderNotFoundException<T> : Exception
{
    public AssetLoaderNotFoundException(AssetInfo assetInfo)
        : base($"No loader found that can load asset of type {typeof(T).Name} with {assetInfo.Extension} extension.")
    {
    }

    public AssetLoaderNotFoundException()
        : base($"No loader found that can load asset of type {typeof(T).Name}.")
    {
    }
}