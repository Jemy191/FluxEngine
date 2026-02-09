using Flux.Assets;
using Flux.Assets.FileChangeWatchers;
using Flux.Ecs;
using Flux.Rendering.Assets;
using Flux.Rendering.GLPrimitives;
using Flux.Rendering.Services;
using Flux.Resources;
using Flux.Resources.ResourceHandles;
using JetBrains.Annotations;

namespace Flux.Rendering.ResourceManagers;

[PublicAPI]
public sealed class ShaderResourceManager : ResourceManager<ShaderCreationInfo, Shader>
{
    readonly LoadingService loadingService;
    readonly AssetManager assetManager;
    readonly IFileChangeWatcher fileChangeWatcher;

    public ShaderResourceManager(
        IEcsWorldService ecsWorldService,
        LoadingService loadingService,
        ResourcesRepository resourcesRepository,
        AssetManager assetManager,
        IFileChangeWatcher fileChangeWatcher)
        : base(ecsWorldService, resourcesRepository)
    {
        this.loadingService = loadingService;
        this.assetManager = assetManager;
        this.fileChangeWatcher = fileChangeWatcher;
    }

    protected override ResourceHandle<Shader> Load(ShaderCreationInfo info, ResourcesRepository resourcesRepository)
    {
        var handle = LoadShader().AsHandle();

        fileChangeWatcher.RegisterFile(assetManager.ResolvePath(info.ShaderAsset), Refresh);

        return handle;

        void Refresh()
        {
            try
            {
                var shader = LoadShader();
                handle.Refresh(shader);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Shader refresh failed: {e.Message}");
            }
        }

        Shader LoadShader()
        {

            return loadingService.LoadShader(assetManager.Load<ShaderAsset>(info.ShaderAsset), info.EntryPoints);
        }
    }

    protected override void Unload(ShaderCreationInfo info, ResourceHandle<Shader> resource)
    {
        fileChangeWatcher.UnregisterFile(assetManager.ResolvePath(info.ShaderAsset));
    }
}