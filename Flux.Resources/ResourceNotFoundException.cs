using Flux.Abstraction;

namespace Flux.Resources;

public class ResourceNotFoundException<T> : Exception where T : IResource
{
    internal ResourceNotFoundException(ResourceId<T> id) : base($"Resource with id {id.Value} not found.") { }
}