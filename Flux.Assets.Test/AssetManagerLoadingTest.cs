using Microsoft.Extensions.Options;

namespace Flux.Assets.Test;

public class AssetManagerLoadingTest
{
    readonly AssetManagerWithId assetManager;

    public AssetManagerLoadingTest()
    {
        var options = AssetManager.AssetManagerOptions.Default;

        assetManager = new AssetManagerWithId(
            [new DummyAssetLoader()],
            Options.Create(options),
            AssetIdTools.GenerateMapping(options.AssetsPath));
    }

    [Test]
    public async Task LoadAssetByFileName()
    {
        const string assetName = "Dummy.txt";
        var asset = assetManager.Load<DummyAsset>(assetName.ToAsset());

        var assetFilePath = Path.Join("Assets", assetName);
        await Assert.That(asset.Asset.Text).IsEqualTo(await File.ReadAllTextAsync(assetFilePath));
    }

    [Test]
    public async Task LoadAssetById()
    {
        const string assetName = "Dummy.txt";
        var id = new AssetId<DummyAsset>(Guid.Parse("df5a8e92-2e64-47db-8f21-a5415453d883"));
        var asset = assetManager.Load(id);

        var assetFilePath = Path.Join("Assets", assetName);
        await Assert.That(asset.Asset.Text).IsEqualTo(await File.ReadAllTextAsync(assetFilePath));
    }
}