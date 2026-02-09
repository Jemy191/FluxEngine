using Flux.Assets.Interfaces;
using Flux.Core;

namespace Flux.Assets.Test;

public class DummyAssetLoader : IAssetLoader<DummyAsset>
{
    public HashSet<FileExtension> SupportedExtensions => [new FileExtension("txt")];
    public DummyAsset Load(FileInfo file) => new DummyAsset(File.ReadAllText(file.FullName));
}