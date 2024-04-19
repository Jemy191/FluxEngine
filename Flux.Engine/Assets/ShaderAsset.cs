namespace Flux.Engine.Assets;

public class ShaderAsset : Asset.Asset
{
    public readonly IReadOnlyDictionary<ShaderStage, string> StageCodes;
    
    public ShaderAsset(IReadOnlyDictionary<ShaderStage, string> stageCodes)
    {
        StageCodes = stageCodes;
    }
}