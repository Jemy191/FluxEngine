using JetBrains.Annotations;

namespace Flux.Asset.Interface;

public interface IAssetImporter
{
    IEnumerable<string> SupportedFileFormats { get; }
    
    [MustDisposeResource]
    Task<IAsset?> Import(Stream stream, string name, string format);
}