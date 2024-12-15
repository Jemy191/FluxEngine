using Silk.NET.OpenGL;

namespace Flux.Rendering.Resources;

public readonly struct NewTexture
{
    readonly uint handle;
    readonly GL gl;

    public NewTexture(GL gl)
    {
        this.gl = gl;

        handle = this.gl.GenTexture();
    }

    public void Bind(TextureUnit textureSlot = TextureUnit.Texture0)
    {
        gl.ActiveTexture(textureSlot);
        gl.BindTexture(TextureTarget.Texture2D, handle);
    }
}