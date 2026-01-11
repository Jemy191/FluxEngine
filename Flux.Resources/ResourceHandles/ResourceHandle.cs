using System.Runtime.CompilerServices;
using Flux.Abstraction;
using JetBrains.Annotations;

namespace Flux.Resources.ResourceHandles;

public class ResourceHandle<TResource> : IResourceHandleInternal where TResource : IResource
{
    [HandlesResourceDisposal]
    public TResource Resource { get; private set; }

    public ResourceHandle(TResource resource) => Resource = resource;

    /// <summary> Replace and dispose of the old resource. </summary>
    public void Refresh(TResource resource)
    {
        var oldResource = Resource;
        Resource = resource;
        oldResource.Dispose();
    }

    void IDisposable.Dispose() => Resource.Dispose();
}