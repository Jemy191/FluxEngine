using Flux.Rendering.GLPrimitives;
using Flux.Rendering.GLPrimitives.Textures;
using Flux.Resources;

namespace Flux.Rendering.ResourceManagers;

public readonly struct MaterialCreationInfo
{
    public readonly ResourceId<Shader> Shader;
    public readonly (uint binding, ResourceId<Texture> texture)[] Textures;

    public MaterialCreationInfo(ResourceId<Shader> shader, (uint binding, ResourceId<Texture> texture)[] textures)
    {
        Shader = shader;
        Textures = textures;
    }
}