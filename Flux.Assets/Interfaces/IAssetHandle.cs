namespace Flux.Assets.Interfaces;

public interface IAssetHandle : IDisposable;

interface IAssetHandleInternal : IAssetHandle
{
    void AddRef();
}