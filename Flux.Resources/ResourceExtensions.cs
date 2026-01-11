using Flux.Abstraction;
using Flux.Resources.ResourceHandles;

namespace Flux.Resources;

public static class ResourceExtensions
{
    extension<TResource>(TResource resource) where TResource : IResource
    {
        public ResourceHandle<TResource> AsHandle() => new ResourceHandle<TResource>((TResource)resource);
    }
}