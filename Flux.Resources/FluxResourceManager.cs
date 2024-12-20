using DefaultEcs;
using DefaultEcs.Resource;
using Flux.Abstraction;
using Flux.Ecs;

namespace Flux.Resources;

public abstract class FluxResourceManager<TInfo, TResource> : AResourceManager<ResourceCreationInfo<TInfo, TResource>, WrapperTemp<TResource>>, IFluxResourceManager where TResource : IResource
{
    readonly World world;
    
    protected FluxResourceManager(IEcsWorldService ecsWorldService)
    {
        world = ecsWorldService.World;
        Manage(world);
    }
    
    protected abstract TResource OnLoad(TInfo info, ResourcesRepository resourcesRepository);
    
    protected override WrapperTemp<TResource> Load(ResourceCreationInfo<TInfo, TResource> info) => new WrapperTemp<TResource>(OnLoad(info.Info, info.ResourcesRepository));
    protected override void OnResourceLoaded(in Entity entity, ResourceCreationInfo<TInfo, TResource> info, WrapperTemp<TResource> resource)
    {
        resource.SetId(entity.Get<Guid>());
        entity.Get<ResourcesRepository>().AddResource(info.Id, resource.Resource);
    }
    protected override void Unload(ResourceCreationInfo<TInfo, TResource> info, WrapperTemp<TResource> resource)
    {
        var repo = world.GetEntities()
            .With<ResourcesRepository>()
            .With((in Guid guid) => guid == resource.EntityId)
            .AsSet()
            .GetEntities()
            .ToArray()
            .Single()
            .Get<ResourcesRepository>();
        
        repo.RemoveResource(info.Id);
        base.Unload(info, resource);
    }
}