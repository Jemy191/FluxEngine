using Flux.Abstraction;

namespace Flux.Resources;

public class ResourceNotRegisteredException<T> : Exception where T : IResource
{
    internal ResourceNotRegisteredException(ResourceId<T> id) : base($"Resource with id {id.Value} is not registered.") { }
}