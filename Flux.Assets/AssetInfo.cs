using Flux.Core;

namespace Flux.Assets;

public record AssetInfo(string RelativePath)
{
    public FileExtension Extension => new FileExtension(Path.GetExtension(RelativePath));
}