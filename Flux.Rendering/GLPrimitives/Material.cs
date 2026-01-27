using Flux.Abstraction;
using Flux.Rendering.ResourceManagers;
using Flux.Resources;
using Flux.Resources.ResourceHandles;
using Silk.NET.OpenGL;
using Texture = Flux.Rendering.GLPrimitives.Textures.Texture;

namespace Flux.Rendering.GLPrimitives;

public readonly struct Material : IResource<MaterialCreationInfo>
{
    readonly ResourceHandle<Shader> shaderHandle;
    readonly (uint binding, ResourceHandle<Texture> texture)[] textures;

    public Material(ResourceId<Shader> shaderId, (uint binding, ResourceId<Texture> texture)[] textures, ResourcesRepository resourcesRepository)
    {
        shaderHandle = resourcesRepository.Get(shaderId);
        this.textures = textures
            .Select(t => (t. binding, resourcesRepository.Get(t.texture)))
            .ToArray();
    }

    public void Use()
    {
        shaderHandle.Resource.Use();

        foreach (var (binding, texture) in textures)
        {
            texture.Resource.Bind(TextureUnit.Texture0 + (int)binding);
        }
    }

    public void Dispose()
    {
        shaderHandle.Resource.Dispose();
        foreach (var texture in textures)
        {
            texture.texture.Resource.Dispose();
        }
    }
}