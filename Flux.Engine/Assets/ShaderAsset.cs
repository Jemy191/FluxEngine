using Flux.Asset;

namespace Flux.Engine.Assets;

public class ShaderAsset : SourceAsset
{
    public required IReadOnlyDictionary<ShaderStage, string> StageCodes { get; init; }
}