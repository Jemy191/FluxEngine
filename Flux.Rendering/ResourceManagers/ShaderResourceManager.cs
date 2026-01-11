using Flux.Ecs;
using Flux.Rendering.GLPrimitives;
using Flux.Rendering.Services;
using Flux.Resources;
using Flux.Resources.FileChangeWatchers;
using Flux.Resources.ResourceHandles;
using JetBrains.Annotations;

namespace Flux.Rendering.ResourceManagers;

[PublicAPI]
public sealed class ShaderResourceManager : FluxResourceManager<(FileInfo vertexFile, FileInfo fragmentFile), Shader>
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

    protected override ResourceHandle<Shader> Load((FileInfo vertexFile, FileInfo fragmentFile) info, ResourcesRepository resourcesRepository)
    {
        var (vertexFile, fragmentFile) = info;

        var handle = LoadShader().AsHandle();

        fileChangeWatcher.RegisterFile(vertexFile, Refresh);
        fileChangeWatcher.RegisterFile(fragmentFile, Refresh);

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

        Shader LoadShader() => loadingService.LoadShader(vertexFile, fragmentFile);
    }

    protected override void Unload((FileInfo vertexFile, FileInfo fragmentFile) info, ResourceHandle<Shader> resource)
    {
        fileChangeWatcher.UnregisterFile(info.vertexFile);
        fileChangeWatcher.UnregisterFile(info.fragmentFile);
    }
}