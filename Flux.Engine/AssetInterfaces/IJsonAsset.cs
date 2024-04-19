using System.Text.Json;
using Flux.Asset;

namespace Flux.Engine.AssetInterfaces;

public interface IJsonAsset : IAsset
{
    JsonElement Root { get; }
}