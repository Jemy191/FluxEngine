using Flux.Ecs;
using Flux.Rendering.GLPrimitives;
using Flux.Rendering.Services;
using Flux.Resources;

namespace Flux.Rendering.ResourceManagers;

public class TextureResourceManager : FluxResourceManager<FileInfo, Texture>
{
    readonly LoadingService loadingService;

    public TextureResourceManager(IEcsWorldService ecsWorldService, LoadingService loadingService) : base(ecsWorldService) =>
        this.loadingService = loadingService;

    protected override Texture OnLoad(FileInfo info, ResourcesRepository resourcesRepository) =>
        loadingService.LoadTexture(info);
}