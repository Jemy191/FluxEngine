using DefaultEcs;
using DefaultEcs.Resource;
using Flux.Abstraction;
using Flux.Ecs;

namespace Flux.Resources;

public abstract class FluxResourceManager<TInfo, TResource> : AResourceManager<Resource<TResource>, Resource<TResource>>, IFluxResourceManager where TResource : IResource
{
    readonly ResourcesRepository resourcesRepository;
    
    protected FluxResourceManager(IEcsWorldService ecsWorldService, ResourcesRepository resourcesRepository)
    {
        this.resourcesRepository = resourcesRepository;
        Manage(ecsWorldService.World);
    }

    protected abstract internal TResource Load(TInfo info, ResourcesRepository resourcesRepository);
    
    protected override sealed Resource<TResource> Load(Resource<TResource> id)
    {
        resourcesRepository.Load(id, this);
        return id;
    }
    
    /// <summary>This function is useless for now.</summary>
    protected override sealed void OnResourceLoaded(in Entity entity, Resource<TResource> id, Resource<TResource> _) { }
    
    protected override sealed void Unload(Resource<TResource> id, Resource<TResource> _) => resourcesRepository.Unload(id);
}