using JetBrains.Annotations;

namespace Flux.Asset.Interface;

public interface IAssetImporter
{
    IEnumerable<string> SupportedFileFormats { get; }
    
    [MustDisposeResource]
    Task<SourceAsset?> Import(Stream stream, Guid guid, string name, string format);
}