using Flux.Rendering.GLPrimitives;
using Flux.Rendering.GLPrimitives.Textures;
using Flux.Resources;

namespace Flux.Rendering.ResourceManagers;

public readonly struct MaterialCreationInfo
{
    public readonly ResourceId<Shader> Shader;
    public readonly (string uniformName, ResourceId<Texture> texture)[] Textures;
    public readonly Uniform[] Uniforms;

    public MaterialCreationInfo(ResourceId<Shader> shader, (string uniformName, ResourceId<Texture> texture)[] textures, Uniform[] uniforms)
    {
        Shader = shader;
        Textures = textures;
        Uniforms = uniforms;
    }
}