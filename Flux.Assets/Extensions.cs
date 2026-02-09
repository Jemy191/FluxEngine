using Flux.Assets.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Flux.Assets;

public static class Extensions
{
    extension(Guid guid)
    {
        internal NoTypeAssetId AsAssetId() => new NoTypeAssetId(guid);
    }

    extension(IServiceCollection services)
    {
        public IServiceCollection AddAssetManagement() => services.AddSingleton<AssetManager>();
        public IServiceCollection AddAssetLoader<TLoader, TAsset>() where TLoader : class, IAssetLoader<TAsset> => services.AddSingleton<IAssetLoader, TLoader>();
    }

    extension(string path)
    {
        public AssetInfo ToAsset() => new AssetInfo(path);
        public EngineInternalAssetInfo ToEngineInternalAsset() => new EngineInternalAssetInfo(path);
    }
}