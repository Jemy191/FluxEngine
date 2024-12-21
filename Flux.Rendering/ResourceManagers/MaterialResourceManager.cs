using Flux.Ecs;
using Flux.Rendering.GLPrimitives;
using Flux.Resources;
using JetBrains.Annotations;

namespace Flux.Rendering.ResourceManagers;

[PublicAPI]
public sealed class MaterialResourceManager : FluxResourceManager<(Resource<Shader> shader, (string uniformName, Resource<Texture> texture)[] textures, Uniform[] uniforms), Material>
{
    public MaterialResourceManager(IEcsWorldService ecsWorldService, ResourcesRepository resourcesRepository) : base(ecsWorldService, resourcesRepository) { }

    protected override Material Load((Resource<Shader> shader, (string uniformName, Resource<Texture> texture)[] textures, Uniform[] uniforms) info, ResourcesRepository resourcesRepository) =>
        new Material(info.shader, info.textures, info.uniforms, resourcesRepository);
}