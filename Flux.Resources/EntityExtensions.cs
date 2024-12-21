using DefaultEcs;
using DefaultEcs.Resource;
using Flux.Abstraction;

namespace Flux.Resources;

public static class EntityExtensions
{
    public static Resource<TResource> AddResource<TResource, TInfo>(this Entity entity, TInfo creationInfo) where TResource : IResource
    {
        var repository = entity.World.Get<ResourcesRepository>();

        if(!repository.GetId<TInfo, TResource>(creationInfo, out var id))
            id = Resource<TResource>.New();
        
        entity.Set(ManagedResource<Resource<TResource>>.Create(new ResourceCreationInfo<TInfo, TResource>(id.Value, creationInfo)));
        return id.Value;
    }
}