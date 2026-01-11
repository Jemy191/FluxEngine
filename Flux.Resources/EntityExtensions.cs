using DefaultEcs;
using DefaultEcs.Resource;
using Flux.Abstraction;

namespace Flux.Resources;

public static class EntityExtensions
{
    public static void AddResource<TResource>(this Entity entity, params ResourceId<TResource>[] newResources) where TResource : IResource
    {
        if (entity.Has<ManagedResource<ResourceId<TResource>[], ResourceId<TResource>>>())
            newResources = entity.Get<ManagedResource<ResourceId<TResource>[], ResourceId<TResource>>>().Info.Union(newResources).ToArray();

        entity.Set(ManagedResource<ResourceId<TResource>>.Create(newResources));
    }
}