using Flux.Abstraction;
using Flux.Resources;
using Silk.NET.OpenGL;
using Texture = Flux.Rendering.GLPrimitives.Textures.Texture;

namespace Flux.Rendering.GLPrimitives;

public readonly struct Material : IResource, IDisposable
{
    readonly Shader shader;
    readonly (string uniformName, Texture texture)[] textures;
    readonly Uniform[] uniforms;

    public Material(Resource<Shader> shader, (string uniformName, Resource<Texture> texture)[] textures, Uniform[] uniforms, ResourcesRepository resourcesRepository)
    {
        this.shader = resourcesRepository.Get(shader);
        this.textures = textures.Select(t => (t.uniformName, resourcesRepository.Get(t.texture))).ToArray();
        this.uniforms = uniforms;
    }

    public void Use()
    {
        shader.Use();

        for (var i = 0; i < textures.Length; i++)
        {
            var (uniformName, texture) = textures[i];
            texture.Bind(TextureUnit.Texture0 + i);
            shader.SetUniform(uniformName, i);
        }

        shader.SetUniforms(uniforms);
    }

    public void SetUniforms(IEnumerable<Uniform> uniforms) => shader.SetUniforms(uniforms);
    public void SetUniform(Uniform uniform) => shader.SetUniform(uniform);

    public void Dispose()
    {
        shader.Dispose();
        foreach (var texture in textures)
        {
            texture.texture.Dispose();
        }
    }
}
