using System.Text;
using Flux.Asset;
using Flux.Asset.Interface;
using Flux.Engine.Assets;

namespace Flux.Engine.AssetImporters;

public class ShaderImporter : IAssetImporter
{
    const string StageToken = "#stage";
    public IEnumerable<string> SupportedFileFormats => ["fluxshader"];
    public async Task<SourceAsset?> Import(Stream stream, Guid guid, string name, string format)
    {
        using var reader = new StreamReader(stream);

        var stringBuilder = new StringBuilder();
        Dictionary<ShaderStage, string> shaderStages = [];
        ShaderStage? currentShaderStage = null;
        
        for (var line = ""; line is not null; line = await reader.ReadLineAsync())
        {
            if (!line.StartsWith(StageToken))
            {
                stringBuilder.AppendLine(line);
                continue;
            }
            
            if (currentShaderStage is not null)
                AddStage(shaderStages, currentShaderStage.Value, stringBuilder);
            
            if (!Enum.TryParse<ShaderStage>(line.Split()[1], out var shaderStage))
                throw new Exception($"Error: Unknown shader stage: {line.Split()[1]}");
            
            currentShaderStage = shaderStage;
        }

        if (currentShaderStage is not null)
            AddStage(shaderStages, currentShaderStage.Value, stringBuilder);

        return new ShaderAsset
        {
            StageCodes = shaderStages
        };
    }
    
    static void AddStage(Dictionary<ShaderStage, string> shaderStages, ShaderStage currentShaderStage, StringBuilder builder)
    {
        if (!shaderStages.TryAdd(currentShaderStage, builder.ToString()))
            throw new Exception("Shader contain two of the same stage");
        builder.Clear();
    }
}