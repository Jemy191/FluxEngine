using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Flux.Core;

namespace Flux.Assets.Interfaces;

public interface IAssetLoader
{
    /// <summary> The file extensions that this loader supports. </summary>
    HashSet<FileExtension> SupportedExtensions { get; }
    bool IsTypeSupported<T>();
    T Load<T>(FileInfo file);
}

/// <summary> This interface implement <see cref="IAssetLoader.SupportedTypes"/> and make <see cref="IAssetLoader.Load"/> typesafe. /summary>
public interface IAssetLoader<out TAsset> : IAssetLoader
{
    bool IAssetLoader.IsTypeSupported<T>() => typeof(TAsset) == typeof(T);
    T IAssetLoader.Load<T>(FileInfo file) => (T)(object)Load(file)!; // Weird hack to make the compiler happy
    TAsset Load(FileInfo file);
}