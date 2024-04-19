using Flux.Asset;

namespace Flux.Engine.AssetInterfaces;

public interface IShaderAsset : IAsset
{
    IReadOnlyDictionary<ShaderStage, string> StageCodes { get; }
}