using System.Numerics;
using Flux.Rendering.Extensions;
using Flux.Rendering.GLPrimitives.Textures;
using Silk.NET.OpenGL;

namespace Flux.Rendering.Services;

public partial class OpenGLRenderService
{
    static readonly FileInfo screenVertexShader = Path.Join("Rendering", "FirstPassScreen.vert").ToEngineInternalAsset();
    static readonly FileInfo screenFragmentShader = Path.Join("Rendering", "FirstPassScreen.frag").ToEngineInternalAsset();
    static readonly FileInfo onlyPositionVertexShader = Path.Join("Rendering", "OnlyPosition.vert").ToEngineInternalAsset();
    static readonly FileInfo compositeFragmentShader = Path.Join("Rendering", "WeightedBlendedTransparency", "WBTCompositePass.frag").ToEngineInternalAsset();

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