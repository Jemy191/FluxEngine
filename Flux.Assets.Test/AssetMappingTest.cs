namespace Flux.Assets.Test;

public class AssetMappingTest
{
    [Test]
    public async Task GenerateMapping() => await Verify(AssetIdTools.GenerateMapping(new DirectoryInfo("Assets")))
        .DontScrubGuids()
        .UseFileName("MappingTestResult");
    [Test]
    public async Task LoadMappingFromFile() => await Verify(AssetIdTools.LoadMappingFromFile(new FileInfo("Assets/AssetMapping.mapping")))
        .DontScrubGuids()
        .UseFileName("MappingTestResult");
}