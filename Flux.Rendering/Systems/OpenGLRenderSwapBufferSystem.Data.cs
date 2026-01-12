using System.Numerics;

namespace Flux.Rendering.Systems;

public partial class OpenGLRenderSwapBufferSystem
{
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
            TexCoords = new Vector2(1, 0)
        },
        new SimpleVertex
        {
            Position = new Vector3(1, 1, 0),
            TexCoords = new Vector2(1, 1)
        },
        new SimpleVertex
        {
            Position = new Vector3(1, -1, 0),
            TexCoords = new Vector2(0, 1)
        }
    ];

    static readonly uint[] indices =
    [
        0, 1, 2,
        2, 3, 0
    ];
}