using DefaultEcs;
using DefaultEcs.Resource;
using Flux.Abstraction;

namespace Flux.Resources;

public static class EntityExtensions
{
    public static void AddResource<TResource>(this Entity entity, params Resource<TResource>[] newResources) where TResource : IResource
    {
        if (entity.Has<ManagedResource<Resource<TResource>[], Resource<TResource>>>())
            newResources = entity.Get<ManagedResource<Resource<TResource>[], Resource<TResource>>>().Info.Union(newResources).ToArray();

        entity.Set(ManagedResource<Resource<TResource>>.Create(newResources));
    }
}