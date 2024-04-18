using System.Numerics;
using Flux.Asset.Assets;
using Flux.Asset.Interface;
using ImageMagick;
using Microsoft.VisualBasic;
using Silk.NET.Maths;

namespace Flux.Asset.AssetImporters;

public class TextureImporter : IAssetImporter<TextureAsset>
{
    readonly List<string> extraSupportedFormat = [];

    public IEnumerable<string> SupportedFileFormats => new List<string>
    {
        "jpg",
        "png",
        "tga"
    }.Union(extraSupportedFormat);
    
    public TextureImporter() { }
    public TextureImporter(List<string> extraSupportedFormat)
    {
        this.extraSupportedFormat = extraSupportedFormat;
    }
    
    public async Task<TextureAsset?> Import(Stream stream)
    {
        using var image = new MagickImage();
        await image.ReadAsync(stream);

        var size = new Vector2D<ushort>((ushort)image.Width, (ushort)image.Height);
        var data = image.GetPixels().ToArray();
        
        return new TextureAsset(size, data);
    }
}