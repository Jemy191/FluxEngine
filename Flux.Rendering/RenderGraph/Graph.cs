using Flux.Rendering.GLPrimitives;
using Flux.Resources;

namespace Flux.Rendering.RenderGraph;

public class Graph
{
    readonly Dictionary<Resource<Shader>, ShaderGraph> shaders = [];
    
    public ShaderGraph AddShader(Resource<Shader> shader)
    {
        if (!shaders.TryGetValue(shader, out var shaderGraph))
        {
            shaderGraph = new ShaderGraph();
            shaders.Add(shader, shaderGraph);
        }
        
        return shaderGraph;
    }
}