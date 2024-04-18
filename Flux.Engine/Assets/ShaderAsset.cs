namespace Flux.Engine.Assets;

public class ShaderAsset : Asset.Asset
{
    public readonly ShaderType Type;
    public readonly string Code;
    
    public ShaderAsset(ShaderType type, string code)
    {
        Type = type;
        Code = code;
    }
}