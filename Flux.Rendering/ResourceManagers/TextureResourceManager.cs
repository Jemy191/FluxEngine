using Flux.Ecs;
using Flux.Rendering.GLPrimitives.Textures;
using Flux.Rendering.Services;
using Flux.Resources;
using Flux.Resources.FileChangeWatchers;
using Flux.Resources.ResourceHandles;
using JetBrains.Annotations;

namespace Flux.Rendering.ResourceManagers;

[PublicAPI]
public sealed class TextureResourceManager : FluxResourceManager<(FileInfo file, TextureSetting setting), Texture>
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

    protected override ResourceHandle<Texture> Load((FileInfo file, TextureSetting setting) info, ResourcesRepository resourcesRepository)
    {
        var (file, setting) = info;

        var handle = LoadTexture().AsHandle();

        fileChangeWatcher.RegisterFile(file, Refresh);

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
        Texture LoadTexture() => loadingService.LoadTexture(file, setting);
    }

    protected override void Unload((FileInfo file, TextureSetting setting) info, ResourceHandle<Texture> resource)
    {

    }
}