using Flux.Asset;
using Flux.Asset.Interface;
using Flux.Engine.Assets;
using ImageMagick;
using Silk.NET.Maths;

namespace Flux.Engine.AssetImporters;

public class TextureImporter : IAssetImporter
{
    public IEnumerable<string> SupportedFileFormats =>
    [
        "jpg",
        "png",
        "tga"
    ];

    public async Task<IAsset?> Import(Stream stream, string name, string format)
    {
        using var image = new MagickImage();
        await image.ReadAsync(stream);

        var size = new Vector2D<ushort>((ushort)image.Width, (ushort)image.Height);
        var data = image.GetPixels().ToArray();

        return new TextureAsset(size, data);
    }
}