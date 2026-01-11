using Flux.Ecs;
using Flux.Rendering.GLPrimitives;
using Flux.Rendering.GLPrimitives.Textures;
using Flux.Resources;
using Flux.Resources.ResourceHandles;
using JetBrains.Annotations;

namespace Flux.Rendering.ResourceManagers;

[PublicAPI]
public sealed class MaterialResourceManager : FluxResourceManager<(ResourceId<Shader> shader, (string uniformName, ResourceId<Texture> texture)[] textures, Uniform[] uniforms), Material>
{
    public MaterialResourceManager(IEcsWorldService ecsWorldService, ResourcesRepository resourcesRepository) : base(ecsWorldService, resourcesRepository) { }

    protected override ResourceHandle<Material> Load((ResourceId<Shader> shader, (string uniformName, ResourceId<Texture> texture)[] textures, Uniform[] uniforms) info, ResourcesRepository resourcesRepository) =>
        new Material(info.shader, info.textures, info.uniforms, resourcesRepository).AsHandle();
}