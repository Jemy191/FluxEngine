using Flux.Abstraction;

namespace Flux.Resources;

public class ResourcesRepository
{
    readonly Dictionary<Guid, IResource> resources = [];

    public void AddResource<T>(Resource<T> id, T resource) where T : IResource
    {
        if (!resources.TryAdd(id.Value, resource))
            throw new ResourceAlreadyExistsException<T>(id);
    }

    public T GetResource<T>(Resource<T> id) where T : IResource
    {
        if (resources.TryGetValue(id.Value, out var resource))
            return (T)resource;

        throw new ResourceNotFoundException<T>(id);
    }
    
    public void RemoveResource<T>(Resource<T> id) where T : IResource
    {
        if (!resources.Remove(id.Value))
            throw new ResourceNotFoundException<T>(id);
    }
}