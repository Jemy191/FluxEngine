using Silk.NET.OpenGL;

namespace Flux.Rendering;

public readonly struct Texture : IDisposable
{
    readonly uint handle;
    readonly GL gl;

    public unsafe Texture(GL gl, Span<byte> data, uint width, uint height)
    {
        this.gl = gl;

        handle = this.gl.GenTexture();
        Bind();

        fixed (void* d = &data[0])
        {
            // Format rgb8 for now until I upgrade the texture system
            this.gl.TexImage2D(TextureTarget.Texture2D, 0, (int)InternalFormat.Rgb8, width, height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, d);
            SetParameters();
        }
    }

    void SetParameters()
    {
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.Repeat);
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.Repeat);
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.LinearMipmapLinear);
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 8);

        gl.GenerateMipmap(TextureTarget.Texture2D);
    }

    public void Bind(TextureUnit textureSlot = TextureUnit.Texture0)
    {
        gl.ActiveTexture(textureSlot);
        gl.BindTexture(TextureTarget.Texture2D, handle);
    }

    public void Dispose() => gl.DeleteTexture(handle);
}