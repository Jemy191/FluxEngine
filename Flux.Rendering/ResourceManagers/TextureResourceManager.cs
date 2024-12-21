using Flux.Ecs;
using Flux.Rendering.GLPrimitives;
using Flux.Rendering.Services;
using Flux.Resources;
using JetBrains.Annotations;

namespace Flux.Rendering.ResourceManagers;

[PublicAPI]
public sealed class TextureResourceManager : FluxResourceManager<FileInfo, Texture>
{
    readonly LoadingService loadingService;

    public TextureResourceManager(IEcsWorldService ecsWorldService, LoadingService loadingService, ResourcesRepository resourcesRepository) : base(ecsWorldService, resourcesRepository) =>
        this.loadingService = loadingService;

    protected override Texture Load(FileInfo info, ResourcesRepository resourcesRepository) =>
        loadingService.LoadTexture(info);
}