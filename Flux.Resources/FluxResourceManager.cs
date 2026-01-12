using DefaultEcs;
using DefaultEcs.Resource;
using Flux.Abstraction;
using Flux.Ecs;
using Flux.Resources.ResourceHandles;

namespace Flux.Resources;

/// <typeparam name="TInfo">That same type that go in the <see cref="IResource{TIngo}"/></typeparam>
/// <typeparam name="TResource"></typeparam>
public abstract class FluxResourceManager<TInfo, TResource> : AResourceManager<ResourceId<TResource>, ResourceId<TResource>>, IFluxResourceManager where TResource : IResource<TInfo>
{
    readonly ResourcesRepository resourcesRepository;
    
    protected FluxResourceManager(IEcsWorldService ecsWorldService, ResourcesRepository resourcesRepository)
    {
        this.resourcesRepository = resourcesRepository;
        Manage(ecsWorldService.World);
    }

    protected abstract internal ResourceHandle<TResource> Load(TInfo info, ResourcesRepository resourcesRepository);
    protected virtual internal void Unload(TInfo info, ResourceHandle<TResource> resource)
    {

    }

    protected override sealed ResourceId<TResource> Load(ResourceId<TResource> id)
    {
        resourcesRepository.Load(id, this);
        return id;
    }

    /// <summary>This function is useless for now.</summary>
    protected override sealed void OnResourceLoaded(in Entity entity, ResourceId<TResource> id, ResourceId<TResource> _) { }

    protected override sealed void Unload(ResourceId<TResource> id, ResourceId<TResource> _) => resourcesRepository.Unload(id, this);
}