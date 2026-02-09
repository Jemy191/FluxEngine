using Flux.Core;

namespace Flux.Assets.Exceptions;

public class AssetLoaderRegistrationException : Exception
{
    public AssetLoaderRegistrationException(FileExtension extension, Type loaderType)
    : base($"Asset loader of type {loaderType.Name} is already registered for file extension .{extension}.")
    {
    }
    public AssetLoaderRegistrationException(Type assetType, Type loaderType)
    : base($"Asset loader of type {loaderType.Name} is already registered for asset type {assetType.Name}.")
    {
    }
}