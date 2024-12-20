using Flux.Abstraction;

namespace Flux.Resources;

public class WrapperTemp<T> where T : IResource
{
    internal readonly T Resource;
    internal Guid? EntityId { get; private set; }
    internal WrapperTemp(T resource) => Resource = resource;

    internal void SetId(Guid id) => EntityId = id;
}