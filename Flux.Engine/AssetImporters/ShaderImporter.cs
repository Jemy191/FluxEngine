using System.Xml;
using Flux.Asset.Interface;
using Flux.Engine.Assets;

namespace Flux.Engine.AssetImporters;

public class ShaderImporter : IAssetImporter<ShaderAsset>
{
    static readonly IReadOnlyDictionary<string, ShaderType> formatToShaderType = new Dictionary<string, ShaderType>
    {
        {"vert", ShaderType.Vertex},
        {"tesc", ShaderType.TessellationControl},
        {"tese", ShaderType.TessellationEvaluation},
        {"geo", ShaderType.Geometry},
        {"frag", ShaderType.Fragment},
        {"comp", ShaderType.Compute},
        {"glsl", ShaderType.Include},
    };
    
    public IEnumerable<string> SupportedFileFormats =>
    [
        "vert",
        "tesc",
        "tese",
        "geo",
        "frag",
        "comp",
        "glsl"
    ];
    public async Task<ShaderAsset?> Import(Stream stream, string name, string format)
    {
        var type = formatToShaderType[format];
        using var reader = new StreamReader(stream);
        var code = await reader.ReadToEndAsync();

        return new ShaderAsset(type, code);
    }
}