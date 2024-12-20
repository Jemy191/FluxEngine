using Flux.Ecs;
using Flux.Rendering.GLPrimitives;
using Flux.Rendering.Services;
using Flux.Resources;

namespace Flux.Rendering.ResourceManagers;

public class ShaderResourceManager : FluxResourceManager<(FileInfo vertexFile, FileInfo fragmentFile), Shader>
{
    readonly LoadingService loadingService;

    public ShaderResourceManager(IEcsWorldService ecsWorldService, LoadingService loadingService) : base(ecsWorldService) =>
        this.loadingService = loadingService;
    
    protected override Shader OnLoad((FileInfo vertexFile, FileInfo fragmentFile) info, ResourcesRepository resourcesRepository) =>
        loadingService.LoadShader(info.vertexFile, info.fragmentFile);
}