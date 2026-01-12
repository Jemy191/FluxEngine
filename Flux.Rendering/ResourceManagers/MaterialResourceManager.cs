using Flux.Ecs;
using Flux.Rendering.GLPrimitives;
using Flux.Rendering.GLPrimitives.Textures;
using Flux.Resources;
using Flux.Resources.ResourceHandles;
using JetBrains.Annotations;

namespace Flux.Rendering.ResourceManagers;

[PublicAPI]
public sealed class MaterialResourceManager : FluxResourceManager<MaterialCreationInfo, Material>
{
    public MaterialResourceManager(IEcsWorldService ecsWorldService, ResourcesRepository resourcesRepository) : base(ecsWorldService, resourcesRepository) { }

    protected override ResourceHandle<Material> Load(MaterialCreationInfo info, ResourcesRepository resourcesRepository) =>
        new Material(info.Shader, info.Textures, info.Uniforms, resourcesRepository).AsHandle();
}