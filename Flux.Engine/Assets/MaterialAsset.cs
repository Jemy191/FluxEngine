namespace Flux.Engine.Assets;

public class MaterialAsset : ResourceAsset
{
    public required ShaderAsset Shader { get; init; }
    public required List<TextureAsset> Textures { get; init; }
}