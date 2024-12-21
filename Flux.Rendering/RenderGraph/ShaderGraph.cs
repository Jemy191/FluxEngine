using Flux.Rendering.GLPrimitives;
using Flux.Resources;

namespace Flux.Rendering.RenderGraph;

public class ShaderGraph
{
    readonly HashSet<Resource<Texture>> textures = [];

    public void AddShader(Resource<Texture> texture) => textures.Add(texture);
}