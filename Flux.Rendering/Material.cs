using Silk.NET.OpenGL;

namespace Flux.Rendering;

public readonly struct Material : IDisposable
{
    readonly Shader shader;
    readonly (string uniformName, Texture texture)[] textures;
    readonly Uniform[] uniforms;

    public Material(Shader shader, (string uniformName, Texture texture)[] textures, Uniform[] uniforms)
    {
        this.shader = shader;
        this.textures = textures;
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

    public void Dispose()
    {
        shader.Dispose();
        foreach (var texture in textures)
        {
            texture.texture.Dispose();
        }
    }
}
