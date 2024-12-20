using Flux.Abstraction;

namespace Flux.Resources;

public class ResourceAlreadyExistsException<T> : Exception where T : IResource
{
    public ResourceAlreadyExistsException(Resource<T> resource) : base($"Resource with ID {resource.Value} already exists.") { }
}