using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Flux.Rendering.Resources;

public class TexturesManager : ResourcesManager<NewTexture>
{
    readonly GL gl;

    public TexturesManager(GL gl)
    {
        this.gl = gl;
    }

    public override unsafe NewTexture Load(FileInfo file)
    {
        var texture = new NewTexture(gl);
        texture.Bind();

        using var image = Image.Load<Rgba32>(file.FullName);

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

        SetParameters();

        return texture;
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
}