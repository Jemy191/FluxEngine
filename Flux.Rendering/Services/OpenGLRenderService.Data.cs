using System.Numerics;
using Flux.Rendering.Extensions;
using Flux.Rendering.GLPrimitives.Textures;
using Silk.NET.OpenGL;

namespace Flux.Rendering.Services;

public partial class OpenGLRenderService
{
    static readonly FileInfo screenShader = Path.Join("Rendering", "FirstPassScreen.slang").ToEngineInternalAsset();
    static readonly FileInfo compositeShader = Path.Join("Rendering", "WeightedBlendedTransparency", "WBTCompositePass.slang").ToEngineInternalAsset();

    static readonly float[] clearAccum = [0f, 0f, 0f, 0f];
    static readonly float[] clearReveal = [1f, 1f, 1f, 1f];

    static readonly SimpleVertex[] vertices =
    [
        new SimpleVertex
        {
            Position = new Vector3(-1, -1, 0),
            TexCoords = new Vector2(0, 0)
        },
        new SimpleVertex
        {
            Position = new Vector3(-1, 1, 0),
            TexCoords = new Vector2(0, 1)
        },
        new SimpleVertex
        {
            Position = new Vector3(1, 1, 0),
            TexCoords = new Vector2(1, 1)
        },
        new SimpleVertex
        {
            Position = new Vector3(1, -1, 0),
            TexCoords = new Vector2(1, 0)
        }
    ];

    static readonly uint[] indices =
    [
        0, 1, 2,
        2, 3, 0
    ];

    static readonly TextureSetting textureSetting = new TextureSetting
    {
        Mipmap = new MipmapSetting.NoMipmap(),
        WrapModeS = TextureWrapMode.ClampToBorder,
        WrapModeT = TextureWrapMode.ClampToBorder,
        TextureMinFilter = TextureMinFilter.Linear,
        TextureMagFilter = TextureMagFilter.Linear,
    };
}