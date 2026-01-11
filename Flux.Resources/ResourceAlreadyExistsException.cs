using Flux.Abstraction;

namespace Flux.Resources;

public class ResourceAlreadyExistsException<T> : Exception where T : IResource
{
    public ResourceAlreadyExistsException(ResourceId<T> resourceId) : base($"Resource with ID {resourceId.Value} already exists.") { }
}