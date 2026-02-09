using Flux.Assets.FileChangeWatchers;
using Flux.Ecs;
using Flux.Rendering.GLPrimitives.Textures;
using Flux.Rendering.Services;
using Flux.Resources;
using Flux.Resources.ResourceHandles;
using JetBrains.Annotations;

namespace Flux.Rendering.ResourceManagers;

[PublicAPI]
public sealed class TextureResourceManager : ResourceManager<TextureCreationInfo, Texture>
{
    readonly LoadingService loadingService;
    readonly IFileChangeWatcher fileChangeWatcher;

    public TextureResourceManager(
        IEcsWorldService ecsWorldService,
        LoadingService loadingService,
        ResourcesRepository resourcesRepository,
        IFileChangeWatcher fileChangeWatcher)
        : base(ecsWorldService, resourcesRepository)
    {
        this.loadingService = loadingService;
        this.fileChangeWatcher = fileChangeWatcher;
    }

    protected override ResourceHandle<Texture> Load(TextureCreationInfo info, ResourcesRepository resourcesRepository)
    {
        var handle = LoadTexture().AsHandle();

        fileChangeWatcher.RegisterFile(info.File, Refresh);

        return handle;

        void Refresh()
        {
            try
            {
                var texture = LoadTexture();
                handle.Refresh(texture);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Texture refresh failed: {e.Message}");
            }
        }
        Texture LoadTexture() => loadingService.LoadTexture(info.File, info.Setting);
    }

    protected override void Unload(TextureCreationInfo info, ResourceHandle<Texture> resource) => fileChangeWatcher.UnregisterFile(info.File);
}