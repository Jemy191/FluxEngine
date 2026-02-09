using Flux.Assets;
using Flux.Slang;

namespace Flux.Rendering.ResourceManagers;

public readonly record struct ShaderCreationInfo(AssetInfo ShaderAsset, EntryPoint[] EntryPoints);