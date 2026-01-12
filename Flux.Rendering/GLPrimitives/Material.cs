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
    readonly (string uniformName, ResourceHandle<Texture> texture)[] textures;
    readonly Uniform[] uniforms;

    public Material(ResourceId<Shader> shaderId, (string uniformName, ResourceId<Texture> texture)[] textures, Uniform[] uniforms, ResourcesRepository resourcesRepository)
    {
        shaderHandle = resourcesRepository.Get(shaderId);
        this.textures = textures.Select(t => (t.uniformName, resourcesRepository.Get(t.texture))).ToArray();
        this.uniforms = uniforms;
    }

    public void Use()
    {
        var shader = shaderHandle.Resource;

        shader.Use();

        for (var i = 0; i < textures.Length; i++)
        {
            var (uniformName, texture) = textures[i];
            texture.Resource.Bind(TextureUnit.Texture0 + i);
            shader.SetUniform(uniformName, i);
        }

        shader.SetUniforms(uniforms);
    }

    public void SetUniforms(IEnumerable<Uniform> uniforms) => shaderHandle.Resource.SetUniforms(uniforms);
    public void SetUniform(Uniform uniform) => shaderHandle.Resource.SetUniform(uniform);

    public void Dispose()
    {
        shaderHandle.Resource.Dispose();
        foreach (var texture in textures)
        {
            texture.texture.Resource.Dispose();
        }
    }
}