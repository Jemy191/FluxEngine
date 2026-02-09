using Flux.Assets.IncludedLoaders;
using Microsoft.Extensions.Options;

namespace Flux.Assets.Test;

public class IncludedLoaderTest
{
    readonly AssetManager assetManager = new AssetManager(
        [new JsonLoader()],
        Options.Create(AssetManager.AssetManagerOptions.Default));

    [Test]
    public async Task TestJsonLoader()
    {
        var asset =  assetManager.Load<string>(new AssetInfo("Test.json"));
        await Assert.That(asset).IsEqualTo("Hello World");
    }
}