using Flux.Engine.AssetInterfaces;

namespace Flux.Engine.Assets;

public class ShaderAsset : IShaderAsset
{
    public IReadOnlyDictionary<ShaderStage, string> StageCodes { get; }

    public ShaderAsset(IReadOnlyDictionary<ShaderStage, string> stageCodes)
    {
        StageCodes = stageCodes;
    }
}