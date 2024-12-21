using DefaultEcs;
using DefaultEcs.Resource;
using Flux.Abstraction;
using Flux.Ecs;

namespace Flux.Resources;

public abstract class FluxResourceManager<TInfo, TResource> : AResourceManager<ResourceCreationInfo<TInfo, TResource>, Resource<TResource>>, IFluxResourceManager where TResource : IResource
{
    readonly ResourcesRepository resourcesRepository;
    
    protected FluxResourceManager(IEcsWorldService ecsWorldService, ResourcesRepository resourcesRepository)
    {
        this.resourcesRepository = resourcesRepository;
        Manage(ecsWorldService.World);
    }

    abstract internal TResource Load(TInfo info);
    
    protected override Resource<TResource> Load(ResourceCreationInfo<TInfo, TResource> info)
    {
        resourcesRepository.Load(info, this);
        return info.Id;
    }
    protected override sealed void OnResourceLoaded(in Entity entity, ResourceCreationInfo<TInfo, TResource> info, Resource<TResource> id) { }
    protected override void Unload(ResourceCreationInfo<TInfo, TResource> info, Resource<TResource> id) => resourcesRepository.Unload(id, info.Info);
}