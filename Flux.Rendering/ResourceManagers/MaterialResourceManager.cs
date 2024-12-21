using Flux.Ecs;
using Flux.Rendering.GLPrimitives;
using Flux.Resources;

namespace Flux.Rendering.ResourceManagers;

public class MaterialResourceManager : FluxResourceManager<(Resource<Shader> shader, (string uniformName, Resource<Texture> texture)[] textures, Uniform[] uniforms), Material>
{
    public MaterialResourceManager(IEcsWorldService ecsWorldService) : base(ecsWorldService) { }

    protected override Material OnLoad((Resource<Shader> shader, (string uniformName, Resource<Texture> texture)[] textures, Uniform[] uniforms) info, ResourcesRepository resourcesRepository) =>
        new Material(info.shader, info.textures, info.uniforms, resourcesRepository);
}