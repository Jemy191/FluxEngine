using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Flux.Asset;
using Flux.Asset.Interface;
using Flux.Core;
using Flux.Engine.Assets;

namespace Flux.Engine.AssetImporters;

public class ResourceImporter<TAssembly> : IAssetImporter
{
    readonly AssetsService assetsService;
    public ResourceImporter(AssetsService assetsService)
    {
        this.assetsService = assetsService;
    }
    
    public IEnumerable<string> SupportedFileFormats => ["fluxasset"];
    public async Task<SourceAsset?> Import(Stream stream, Guid guid, string name, string format)
    {
        using var jsonDocument = await JsonDocument.ParseAsync(stream);
        var root = jsonDocument.RootElement;

        var typeName = root.GetProperty("Type").GetString();
        if (typeName is null)
            throw new Exception("Resource asset do not contain type name");
            
        var type = GetTypeFromAssembly(typeName);
        
        if (!root.TryGetProperty("AssetData", out var assetData))
            throw new Exception($"Unable to deserialize resource asset for guid: {guid}");

        var asset = RuntimeHelpers.GetUninitializedObject(type);
        
        foreach (var property in type.GetProperties())
        {
            if (!assetData.TryGetProperty(property.Name, out var jsonProperty))
                throw new Exception($"Json do not contain {property.Name} property");

            if (property.PropertyType.InheritFrom<SourceAsset>())
            {
                ResourceAsset? refAsset;
                if (jsonProperty.ValueKind == JsonValueKind.Null)
                    refAsset = null;
                else
                    refAsset = await assetsService.Load<ResourceAsset>(jsonProperty.GetGuid());
                
                property.SetValue(asset, refAsset);
                continue;
            }

            if (jsonProperty.ValueKind == JsonValueKind.Array)
            {
                var guids = jsonProperty.Deserialize<List<Guid>>() ?? [];

                var assets = new List<SourceAsset?>();
                foreach (var assetGuid in guids)
                {
                    assets.Add(await assetsService.Load<SourceAsset>(assetGuid));
                }
                
                
                
                continue;
            }
            var propertyValue = jsonProperty.Deserialize(property.PropertyType);

            property.SetValue(asset, propertyValue);
        }

        return asset as SourceAsset;
    }

    static Type GetTypeFromAssembly(string typeName)
    {
        var currentAssembly = typeof(TAssembly).Assembly;

        foreach (var assembly in currentAssembly.GetReferencedAssemblies().Select(Assembly.Load).Union([currentAssembly]))
        {
            var type = assembly.GetType(typeName, false, true);
            if (type is not null)
                return type;
        }
        
        throw new Exception($"Unable to found type: {typeName} in referenced assemblies");
    }
}