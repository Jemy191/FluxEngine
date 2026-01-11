using Flux.Abstraction;

namespace Flux.Resources.ResourceHandles;

public class ResourceHandle<TResource> : IResourceHandle where TResource : IResource
{
    public TResource Resource { get; private set; }

    public IResource ResourceInterface => Resource;

    public ResourceHandle(TResource resource) => Resource = resource;

    public static implicit operator TResource(ResourceHandle<TResource> handle) => handle.Resource;
    public void Dispose() => Resource.Dispose();
}