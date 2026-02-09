using Flux.Assets.Exceptions;
using Flux.Assets.Interfaces;
using Microsoft.Extensions.Options;

namespace Flux.Assets.Test;

public class AssetManagerIdTest
{
    readonly AssetManagerWithId assetManager;

    public AssetManagerIdTest()
    {
        var options = AssetManager.AssetManagerOptions.Default;

        assetManager = new AssetManagerWithId(
            [new DummyAssetLoader()],
            Options.Create(options),
            AssetIdTools.GenerateMapping(options.AssetsPath));
    }

    [Test]
    public async Task GetExistingAssetId()
    {
        var existingId = new AssetId<DummyAsset>(Guid.Parse("df5a8e92-2e64-47db-8f21-a5415453d883"));
        var assetId = assetManager.GetId<DummyAsset>("Dummy.txt".ToAsset());

        await Assert.That(assetId).IsEqualTo(existingId);
    }

    [Test]
    public async Task GetNonExistingAssetId() => await Assert
        .That(() => assetManager.GetId<DummyAsset>("NonExisting.txt".ToAsset()))
        .Throws<FileNotFoundException>();

    [Test]
    public async Task GetAssetWithoutIdFile() => await Assert
        .That(() => assetManager.GetId<DummyAsset>("DummyWithoutId.txt".ToAsset()))
        .Throws<FileNotFoundException>();

    [Test]
    public async Task GetAssetIdWithoutLoaderForExtension() => await Assert
        .That(() => assetManager.GetId<DummyAsset>("DummyWithoutLoader.png".ToAsset()))
        .Throws<AssetLoaderNotFoundException<DummyAsset>>();

    [Test]
    public async Task GetAssetIdWithoutLoaderForType() => await Assert
        .That(() => assetManager.GetId<DummyAssetWithoutLoader>("DummyWithoutLoader.png".ToAsset()))
        .Throws<AssetLoaderNotFoundException<DummyAssetWithoutLoader>>();
}