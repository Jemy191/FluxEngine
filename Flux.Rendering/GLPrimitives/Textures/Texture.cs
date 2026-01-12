using Flux.Abstraction;
using Flux.Rendering.ResourceManagers;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Flux.Rendering.GLPrimitives.Textures;

public readonly struct Texture : IResource<TextureCreationInfo>
{
    public readonly uint Handle;
    readonly GL gl;
    readonly TextureSetting setting;

    /// <summary> Create a texture object without data. </summary>
    public Texture(GL gl, TextureSetting setting, InternalFormat internalFormat, Vector2D<uint> size, PixelFormat pixelFormat, PixelType pixelType)
    {
        this.gl = gl;
        this.setting = setting;

        Handle = this.gl.GenTexture();
        Bind();

        unsafe
        {
            this.gl.TexImage2D(
                TextureTarget.Texture2D,
                0,
                internalFormat,
                size.X,
                size.Y,
                0,
                pixelFormat,
                pixelType,
                null
            );
        }
        SetParameters();
    }

    public Texture(GL gl, Image<Rgba32> image, TextureSetting setting)
    {
        this.gl = gl;
        this.setting = setting;

        Handle = this.gl.GenTexture();
        Bind();
        LoadImage(gl, image);

        SetParameters();
    }

    public unsafe Texture(GL gl, Span<byte> data, uint width, uint height, TextureSetting setting)
    {
        this.gl = gl;
        this.setting = setting;

        Handle = this.gl.GenTexture();
        Bind();

        fixed (void* d = &data[0])
        {
            this.gl.TexImage2D(TextureTarget.Texture2D, 0, (int)InternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, d);
            SetParameters();
        }
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

    void SetParameters()
    {
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)setting.WrapModeS);
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)setting.WrapModeT);
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)setting.TextureMinFilter);
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)setting.TextureMagFilter);

        var gl1 = gl;
        setting.Mipmap.MatchMipmap(m =>
        {
            gl1.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, m.TextureBaseLevel);
            gl1.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, m.TextureMaxLevel);

            gl1.GenerateMipmap(TextureTarget.Texture2D);
        }, () => { });

    }

    public void Bind(TextureUnit textureSlot = TextureUnit.Texture0)
    {
        gl.ActiveTexture(textureSlot);
        gl.BindTexture(TextureTarget.Texture2D, Handle);
    }

    public void Dispose() => gl.DeleteTexture(Handle);
}