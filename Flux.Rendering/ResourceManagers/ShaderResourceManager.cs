using Flux.Ecs;
using Flux.Rendering.GLPrimitives;
using Flux.Rendering.Services;
using Flux.Resources;
using JetBrains.Annotations;

namespace Flux.Rendering.ResourceManagers;

[PublicAPI]
public sealed class ShaderResourceManager : FluxResourceManager<(FileInfo vertexFile, FileInfo fragmentFile), Shader>
{
    readonly LoadingService loadingService;

    public ShaderResourceManager(IEcsWorldService ecsWorldService, LoadingService loadingService, ResourcesRepository resourcesRepository) : base(ecsWorldService, resourcesRepository) =>
        this.loadingService = loadingService;
    
    protected override Shader Load((FileInfo vertexFile, FileInfo fragmentFile) info, ResourcesRepository resourcesRepository) =>
        loadingService.LoadShader(info.vertexFile, info.fragmentFile);
}