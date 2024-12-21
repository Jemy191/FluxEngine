using System.Diagnostics.CodeAnalysis;
using Flux.Abstraction;

namespace Flux.Resources;

public class ResourcesRepository
{
    readonly Dictionary<Guid, IResource> resources = [];
    readonly Dictionary<int, Guid> idByCreationInfo = [];

    public T Get<T>(Resource<T> id) where T : IResource
    {
        if (resources.TryGetValue(id.Value, out var resource))
            return (T)resource;

        throw new ResourceNotFoundException<T>(id);
    }

    /// <returns>True if a resource was already loaded with <see cref="creationInfo"/></returns>
    internal bool GetId<TInfo, TResource>(TInfo creationInfo, [NotNullWhen(true)]out Resource<TResource>? id) where TResource : IResource
    {
        id = null;
        var exist = idByCreationInfo.TryGetValue(creationInfo!.GetHashCode(), out var guid);
        if(exist)
            id = new Resource<TResource>(guid);

        return exist;
    }

    internal void Load<TInfo, TResource>(ResourceCreationInfo<TInfo, TResource> info, FluxResourceManager<TInfo, TResource> resourceManager) where TResource : IResource
    {
        if (!resources.TryAdd(info.Id.Value, resourceManager.Load(info.Info)))
            throw new ResourceAlreadyExistsException<TResource>(info.Id);

        idByCreationInfo.Add(info.Info!.GetHashCode(), info.Id.Value);
    }

    internal void Unload<TResource, TInfo>(Resource<TResource> id, TInfo infoInfo) where TResource : IResource
    {
        Get(id).Dispose();
        idByCreationInfo.Remove(infoInfo!.GetHashCode());
        resources.Remove(id.Value);
    }
}