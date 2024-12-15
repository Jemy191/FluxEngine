using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Flux.Rendering;

public readonly struct Texture : IDisposable
{
    readonly uint handle;
    readonly GL gl;

    public Texture(GL gl, Image<Rgba32> image)
    {
        this.gl = gl;

        handle = this.gl.GenTexture();
        Bind();
        LoadImage(gl, image);

        SetParameters();
    }

    static unsafe void LoadImage(GL gl, Image<Rgba32> image)
    {
        gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba8, (uint)image.Width, (uint)image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, null);

        image.ProcessPixelRows(accessor =>
        {
            for (var i = 0; i < accessor.Height; i++)
            {
                fixed (void* data = accessor.GetRowSpan(i))
                {
                    gl.TexSubImage2D(TextureTarget.Texture2D, 0, 0, i, (uint)accessor.Width, 1, PixelFormat.Rgba, PixelType.UnsignedByte, data);
                }
            }
        });
    }

    public unsafe Texture(GL gl, Span<byte> data, uint width, uint height)
    {
        this.gl = gl;

        handle = this.gl.GenTexture();
        Bind();

        fixed (void* d = &data[0])
        {
            this.gl.TexImage2D(TextureTarget.Texture2D, 0, (int)InternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, d);
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