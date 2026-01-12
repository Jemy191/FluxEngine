using Flux.Ecs;
using Flux.Rendering.GLPrimitives;
using Flux.Rendering.Services;
using Flux.Resources;
using Flux.Resources.FileChangeWatchers;
using Flux.Resources.ResourceHandles;
using JetBrains.Annotations;

namespace Flux.Rendering.ResourceManagers;

[PublicAPI]
public sealed class ShaderResourceManager : FluxResourceManager<ShaderCreationInfo, Shader>
{
    readonly LoadingService loadingService;
    readonly IFileChangeWatcher fileChangeWatcher;

    public ShaderResourceManager(
        IEcsWorldService ecsWorldService,
        LoadingService loadingService,
        ResourcesRepository resourcesRepository,
        IFileChangeWatcher fileChangeWatcher)
        : base(ecsWorldService, resourcesRepository)
    {
        this.loadingService = loadingService;
        this.fileChangeWatcher = fileChangeWatcher;
    }

    protected override ResourceHandle<Shader> Load(ShaderCreationInfo info, ResourcesRepository resourcesRepository)
    {
        var handle = LoadShader().AsHandle();

        fileChangeWatcher.RegisterFile(info.VertexFile, Refresh);
        fileChangeWatcher.RegisterFile(info.FragmentFile, Refresh);

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

        Shader LoadShader() => loadingService.LoadShader(info.VertexFile, info.FragmentFile);
    }

    protected override void Unload(ShaderCreationInfo info, ResourceHandle<Shader> resource)
    {
        fileChangeWatcher.UnregisterFile(info.VertexFile);
        fileChangeWatcher.UnregisterFile(info.FragmentFile);
    }
}