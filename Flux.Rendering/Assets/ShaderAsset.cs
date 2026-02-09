namespace Flux.Rendering.Assets;

public class ShaderAsset
{
    public readonly string Vertex;
    public readonly string Fragment;

    public ShaderAsset(string vertex, string fragment)
    {
        Vertex = vertex;
        Fragment = fragment;
    }
}