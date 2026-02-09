using System.Text.Json;
using Flux.Assets.Interfaces;
using Flux.Core;

namespace Flux.Assets.IncludedLoaders;

public class JsonLoader : IAssetLoader
{
    public HashSet<FileExtension> SupportedExtensions => [new FileExtension("json")];
    public bool IsTypeSupported<T>() => true;
    public T Load<T>(FileInfo file) => JsonSerializer.Deserialize<T>(File.ReadAllText(file.FullName)) ?? throw new JsonException($"Failed to deserialize {file.FullName}");
}