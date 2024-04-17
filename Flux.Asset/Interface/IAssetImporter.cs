using JetBrains.Annotations;

namespace Flux.Asset.Interface;

public interface IAssetImporter<T> : IAssetImporter where T : Asset
{
    List<string> SupportedFileFormats { get; }
    
    [MustDisposeResource]
    Task<T?> Import(Stream stream);
}

public interface IAssetImporter
{
    
}